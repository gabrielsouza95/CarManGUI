#include <SPI.h>
#include <Wire.h>
#include <MPU6050.h>
#include <Adafruit_GFX.h> //OLED
#include <Adafruit_SSD1306.h> //OLED
#include <SD.h>
#include "RTClib.h"

///
#define SERIAL_BAUDRATE 9600 
#define LED_PIN 13 
///MPU Releated
const short MPU[3] = {0x68, 0x69, 0x68};

int16_t AcX[3] = {16384,0,16384}, 
        AcY[3] = {16384,0,16384}, 
        AcZ[3] = {16384,0,16384}, 
        GyX[3] = {16384,0,16384}, 
        GyY[3] = {16384,0,16384}, 
        GyZ[3] = {16384,0,16384}; //MPU
        
int16_t Acel_convert = 16384;

int16_t Susp[4] = { 4, 5, 6, 7 };   // posição da suspensão
float   Temp[4] = { 8.1, 9.2, 10.3, 11.4 }; // temp dos discos de freio
int16_t Pressao = 12; // pressao do freio     
int16_t VelRoda = 13;
short MultiplexPin[2] = {22, 23}; //posição 1 = pino A
                                //posição 2 = pino B
short sensor_1 = 0, sensor_2 = 1, sensor_3 = 2, sensor_4 = 3;
  
bool Flash = false, Interrupting = false;
#define INTERRUPT_PIN_PRX 19 //pinos que funcionam: 2, 3, 19, 18
#define INTERRUPT_PIN_ANT 18
#define INTERRUPT_PIN_REC 2
///Display related
#define OLED_RESET 4
Adafruit_SSD1306 display(OLED_RESET);

#define SCREEN_WIDTH 128 // OLED display width, in pixels
#define SCREEN_HEIGHT 64 // OLED display height, in pixels

#define LIN_MENU   0
#define COL_MENU1  0
#define COL_MENU2  33
#define COL_MENU3  95

#define LIN_LABEL1    16
#define LIN_LABEL2    32
#define LIN_LABEL3    48
#define COL_LABEL     0
#define LIN_INFO1     16
#define LIN_INFO2     32
#define LIN_INFO3     48
#define COL_INFO      50

#define TEMP_OFFSET   5
#define SUSP_OFFSET   12

///Opcao selecionada
short opcao = 0;

//Pino CS do cartao SD
int Pino_CS = 10;
File file;
RTC_DS1307 rtc;
bool grava = false;

///MPU Releated
void getGyroValues(int pMultiplex, int pMPU, int16_t *pGyX, int16_t *pGyY, int16_t *pGyZ) 
{ // MPU 6050 stuff
  if(pMultiplex == 1 || pMultiplex == 2) // se for um dos dois MPUs ligados juntos no primeiro grupo do multiplexador
  {
    digitalWrite(MultiplexPin[0], LOW);
    digitalWrite(MultiplexPin[1], LOW);
  }
  else // se não liga no segundo
  {
    digitalWrite(MultiplexPin[0], HIGH);
    digitalWrite(MultiplexPin[1], LOW);   
  }
  Wire.beginTransmission(pMPU);
  Wire.write(0x43);
  Wire.endTransmission(); //talvez colocar false para não dar problema com o MLX
  Wire.requestFrom(pMPU, 6);
  while(Wire.available() < 6);
  *pGyX = Wire.read()<<8|Wire.read(); //0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)  
  *pGyY = Wire.read()<<8|Wire.read(); //0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)  
  *pGyZ = Wire.read()<<8|Wire.read(); //0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)     
}

void getAccelValues(int pMultiplex, int pMPU, int16_t *pAcX, int16_t *pAcY, int16_t *pAcZ) 
{ // MPU 6050 stuff
  if(pMultiplex == 1 || pMultiplex == 2) // se for um dos dois MPUs ligados juntos no primeiro grupo do multiplexador
  {
    digitalWrite(MultiplexPin[0], LOW);
    digitalWrite(MultiplexPin[1], LOW);
  }
  else // se não liga no segundo
  {
    digitalWrite(MultiplexPin[0], HIGH);
    digitalWrite(MultiplexPin[1], LOW);   
  }
  Wire.beginTransmission(pMPU);
  Wire.write(0x3B);
  Wire.endTransmission(); //talvez colocar false para não dar problema com o MLX
  Wire.requestFrom(pMPU, 6);
  while(Wire.available() < 6);
  *pAcX = Wire.read()<<8|Wire.read(); //0x43 (GYRO_XOUT_H) & 0x44 (GYRO_XOUT_L)  
  *pAcY = Wire.read()<<8|Wire.read(); //0x45 (GYRO_YOUT_H) & 0x46 (GYRO_YOUT_L)  
  *pAcZ = Wire.read()<<8|Wire.read(); //0x47 (GYRO_ZOUT_H) & 0x48 (GYRO_ZOUT_L)     
}

void initializeMPU(int pMultiplex, int pMPU)
{
  //Serial.print("Iniciando MPU");
  if(pMultiplex == 1 || pMultiplex == 2) // se for um dos dois MPUs ligados juntos no primeiro grupo do multiplexador
  {
    digitalWrite(MultiplexPin[0], LOW);
    digitalWrite(MultiplexPin[1], LOW);
  }
  else
  {
    digitalWrite(MultiplexPin[0], HIGH);
    digitalWrite(MultiplexPin[1], LOW);   
  }
  //POWER MANAGEMENT RELATED 
  Wire.beginTransmission(pMPU); //escrevendo no endereço do MPU
  Wire.write(0x6B);            //acessando registrador responsável
  Wire.write(0b00000000);      //seta o sleep do MPU-6050 em 0  
  Wire.endTransmission();      //termina a configuração //talvez colocar false para não dar problema com o MLX

  //GYRO CONFIG RELATED
  Wire.beginTransmission(pMPU);
  Wire.write(0x1B);
  Wire.write(0b00000000);
  Wire.endTransmission();//talvez colocar false para não dar problema com o MLX

  //ACCELEROMETER CONFIG RELATED
  Wire.beginTransmission(pMPU);
  Wire.write(0x1C);
  Wire.write(0b00000000);
  Wire.endTransmission();//talvez colocar false para não dar problema com o MLX
}

void readSensorsData(){
  getAccelValues    (sensor_2 + 1,
                     MPU[sensor_2], 
                     &AcX[sensor_2], 
                     &AcY[sensor_2], 
                     &AcZ[sensor_2]);
                     
  getGyroValues     (sensor_2 + 1,
                     MPU[sensor_2], 
                     &GyX[sensor_2], 
                     &GyY[sensor_2], 
                     &GyZ[sensor_2]);
  
}

///

void opcao_mais ()
{
  detachInterrupt(digitalPinToInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_PRX)));
  
   static unsigned long last_interrupt_time = 0;
   unsigned long interrupt_time = millis();
   // If interrupts come faster than 200ms, assume it's a bounce and ignore
   if (interrupt_time - last_interrupt_time > 200)
   {
     opcao++;
     if(opcao > 11)
      opcao = 0;
   }
   last_interrupt_time = interrupt_time;
  
  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_PRX), opcao_mais, FALLING);
}
void opcao_menos ()
{
  detachInterrupt(digitalPinToInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_ANT)));
  
   static unsigned long last_interrupt_time = 0;
   unsigned long interrupt_time = millis();
   // If interrupts come faster than 200ms, assume it's a bounce and ignore
   if (interrupt_time - last_interrupt_time > 200)
   {
     opcao--;
     if(opcao < 0)
      opcao = 11;
   }
   last_interrupt_time = interrupt_time;
  
  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_ANT), opcao_menos, FALLING);
}
void opcao_rec ()
{
  detachInterrupt(digitalPinToInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_REC)));
  
   static unsigned long last_interrupt_time = 0;
   unsigned long interrupt_time = millis();
   // If interrupts come faster than 200ms, assume it's a bounce and ignore
   if (interrupt_time - last_interrupt_time > 200)
     grava = !grava;

   last_interrupt_time = interrupt_time;
  
  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_REC), opcao_rec, FALLING);
}

void mostra_menu()
{
  display.setTextSize(1); // Draw 2X-scale text
  display.setTextColor(WHITE);
  display.setCursor(COL_MENU1, LIN_MENU); //posiciona o cursor para escrever o texto da opção

  switch(opcao)///opcao anterior
  {
    case  0:display.print("SUSPT");break;
    case  1:display.print("ACL 1");break;
    case  2:display.print("ACL 2");break;
    case  3:display.print("ACL 3");break;
    case  4:display.print("GYR 1");break;
    case  5:display.print("GYR 2");break;
    case  6:display.print("GYR 3");break;
    case  7:display.print("TEMP1");break;
    case  8:display.print("TEMP2");break;
    case  9:display.print("FREIO");break;
    case 10:display.print("VEL R");break;
    case 11:display.print("SUSPF");break;
  }
  

  display.setTextSize(2); // Draw 2X-scale text
  display.setTextColor(WHITE);
  display.setCursor(COL_MENU2, LIN_MENU); //posiciona o cursor para escrever o texto da opção

  switch(opcao)///opcao atual
  {
    case  0:display.print("ACL 1");break;
    case  1:display.print("ACL 2");break;
    case  2:display.print("ACL 3");break;
    case  3:display.print("GYR 1");break;
    case  4:display.print("GYR 2");break;
    case  5:display.print("GYR 3");break;
    case  6:display.print("TEMP1");break;
    case  7:display.print("TEMP2");break;
    case  8:display.print("FREIO");break;
    case  9:display.print("VEL R");break;
    case 10:display.print("SUSPF");break;
    case 11:display.print("SUSPT");break;
  }
  if(grava)
    display.print(".");
  
  display.setTextSize(1); // Draw 2X-scale text
  display.setTextColor(WHITE);
  display.setCursor(COL_MENU3, LIN_MENU); //posiciona o cursor para escrever o texto da opção
  
  switch(opcao) ///proxima opcao
  {
    case  0:display.print("ACL 2");break;
    case  1:display.print("ACL 3");break;
    case  2:display.print("GYR 1");break;
    case  3:display.print("GYR 2");break;
    case  4:display.print("GYR 3");break;
    case  5:display.print("TEMP1");break;
    case  6:display.print("TEMP2");break;
    case  7:display.print("FREIO");break;
    case  8:display.print("VEL R");break;
    case  9:display.print("SUSPF");break;
    case 10:display.print("SUSPT");break;
    case 11:display.print("ACL 1");break;
  }
}

void mostra_label()
{
  display.setTextSize(2); // Draw 2X-scale text
  ///labels
  
  display.setCursor(COL_LABEL, LIN_LABEL1);
  switch(opcao) 
  {
    case  0:
    case  1:
    case  2:display.print("A.X:");break;
    case  3:
    case  4:
    case  5:display.print("G.X:");break;
    case  6:
    case  7:display.print("D");display.print((char)247);display.print("C:");break;
    case  8:display.print("PSI:");break;
    case  9:display.print("K/h:");break;
    case 10:
    case 11:display.print("CM:");break; 
  }
  
  display.setCursor(COL_LABEL, LIN_LABEL2);
  switch(opcao)
  {
    case  0:
    case  1:
    case  2:display.print("A.Y:");break;
    case  3:
    case  4:
    case  5:display.print("G.Y:");break;
    case  6:
    case  7:display.print("E");display.print((char)247);display.print("C:");break;//char247 eh o caractere de graus
    case  8:break;
    case  9:break;
    case 10:
    case 11:display.print("CM:");break;  
  }
  
  display.setCursor(COL_LABEL, LIN_LABEL3);
  switch(opcao)
  {
    case  0:
    case  1:
    case  2:display.print("A.Z:");break;
    case  3:
    case  4:
    case  5:display.print("G.Z:");break;
    case  6:
    case  7:break;
    case  8:break;
    case  9:break;
    case 10:
    case 11:break;  
  }  
}

void mostra_info()
{
  ///infos
  display.setCursor(COL_INFO, LIN_INFO1);
  float AcelConv = 0.0000;
  switch(opcao)///opcao atual
  {
    case  0:AcelConv = (AcX[sensor_1]/Acel_convert);display.print(AcelConv);break;
    case  1:AcelConv = (AcX[sensor_2]/Acel_convert);display.print(AcelConv);break;
    case  2:AcelConv = (AcX[sensor_3]/Acel_convert);display.print(AcelConv);break;
    case  3:display.print(GyX[sensor_1]);break;
    case  4:display.print(GyX[sensor_2]);break;
    case  5:display.print(GyX[sensor_3]);break;
    case  6:display.setCursor(COL_INFO, LIN_INFO1);
            display.print(Temp[sensor_1]);break;
    case  7:display.setCursor(COL_INFO - TEMP_OFFSET, LIN_INFO1);
            display.print(Temp[sensor_2]);break;
    case  8:display.print(Pressao);break;
    case  9:display.print(VelRoda);break;
    case 10:display.setCursor(COL_INFO - SUSP_OFFSET, LIN_INFO1);
            display.print(Susp[sensor_1]);break;
    case 11:display.setCursor(COL_INFO - SUSP_OFFSET, LIN_INFO1);
            display.print(Susp[sensor_2]);break;
  }
  

  display.setCursor(COL_INFO, LIN_INFO2);
  switch(opcao)///opcao atual
  {
    case  0:AcelConv = (AcY[sensor_1]/Acel_convert);display.print(AcelConv);break;
    case  1:AcelConv = (AcY[sensor_2]/Acel_convert);display.print(AcelConv);break;
    case  2:AcelConv = (AcY[sensor_3]/Acel_convert);display.print(AcelConv);break;
    case  3:display.print(GyY[sensor_1]);break;
    case  4:display.print(GyY[sensor_2]);break;
    case  5:display.print(GyY[sensor_3]);break;
    case  6:display.setCursor(COL_INFO - TEMP_OFFSET, LIN_INFO2 );
            display.print(Temp[sensor_3]);break;
    case  7:display.setCursor(COL_INFO - TEMP_OFFSET, LIN_INFO2);
            display.print(Temp[sensor_4]);break;
    case  8:break;
    case  9:break;
    case 10:display.setCursor(COL_INFO - SUSP_OFFSET, LIN_INFO2);
            display.print(Susp[sensor_3]);break;
    case 11:display.setCursor(COL_INFO - SUSP_OFFSET, LIN_INFO2);
            display.print(Susp[sensor_4]);break;
  }
  

  display.setCursor(COL_INFO, LIN_INFO3);
  switch(opcao)///opcao atual
  {
    case  0:AcelConv = (AcZ[sensor_1]/Acel_convert);display.print(AcelConv);break;
    case  1:AcelConv = (AcZ[sensor_2]/Acel_convert);display.print(AcelConv);break;
    case  2:AcelConv = (AcZ[sensor_3]/Acel_convert);display.print(AcelConv);break;
    case  3:display.print(GyZ[sensor_1]);break;
    case  4:display.print(GyZ[sensor_2]);break;
    case  5:display.print(GyZ[sensor_3]);break;
    case  6:break;
    case  7:break;
    case  8:break;
    case  9:break;
    case 10:break;
    case 11:break;
  }
}

///
void grava_cartao_SD()
{
  Serial.print("Gravando SD");
  //Abre arquivo no SD para gravacao
  file = SD.open("log.txt", FILE_WRITE);
  //Le as informacoes de data e hora
  DateTime now = rtc.now();
  //Grava data no cartao SD
  file.print("Data/hora: ");
  file.print(now.day(), DEC);
  file.print('/');
  file.print(now.month() < 10 ? "0" : "");
  file.print(now.month(), DEC);
  file.print('/');
  file.print(now.year(), DEC);
  file.print(' ');
  file.print(now.hour() < 10 ? "0" : "");
  file.print(now.hour(), DEC);
  file.print(':');
  file.print(now.minute() < 10 ? "0" : "");
  file.print(now.minute(), DEC);
  file.print(':');
  file.print(now.second() < 10 ? "0" : "");
  file.print(now.second(), DEC);
  file.print(' ');
  //Grava Info no cartao SD
  file.println(AcX[sensor_1]);file.print(':');
  file.println(AcY[sensor_1]);file.print(':');
  file.println(AcZ[sensor_1]);file.print(':');
  file.println(GyX[sensor_1]);file.print(':');
  file.println(GyY[sensor_1]);file.print(':');
  file.println(GyZ[sensor_1]);file.print(':');
  
  file.println(AcX[sensor_2]);file.print(':');
  file.println(AcY[sensor_2]);file.print(':');
  file.println(AcZ[sensor_2]);file.print(':');
  file.println(GyX[sensor_2]);file.print(':');
  file.println(GyY[sensor_2]);file.print(':');
  file.println(GyZ[sensor_2]);file.print(':');
  
  file.println(AcX[sensor_3]);file.print(':');
  file.println(AcY[sensor_3]);file.print(':');
  file.println(AcZ[sensor_3]);file.print(':');
  file.println(GyX[sensor_3]);file.print(':');
  file.println(GyY[sensor_3]);file.print(':');
  file.println(GyZ[sensor_3]);file.print(':');

  file.println(Temp[sensor_1]);file.print(':');
  file.println(Temp[sensor_2]);file.print(':');
  file.println(Temp[sensor_3]);file.print(':');
  file.println(Temp[sensor_4]);file.print(':');

  file.println(Pressao);file.print(':');
  file.println(VelRoda);file.print(':');

  file.println(Susp[sensor_1]);file.print(':');
  file.println(Susp[sensor_2]);file.print(':');
  file.println(Susp[sensor_3]);file.print(':');
  file.println(Susp[sensor_4]);
  //Fecha arquivo
  file.close();
}
///

void setup() {
  pinMode(LED_PIN, OUTPUT);
  Serial.begin(SERIAL_BAUDRATE);
  
  pinMode(MultiplexPin[0], OUTPUT);
  pinMode(MultiplexPin[1], OUTPUT);

  Wire.begin(); 

  initializeMPU(sensor_2 + 1, MPU[sensor_2]);
  
  display.begin(SSD1306_SWITCHCAPVCC, 0x3C);  
  display.display();
  delay(2000); // Pause for 2 seconds

  display.clearDisplay();

  pinMode(10, OUTPUT); // change this to 53 on a mega  // don't follow this!!
  digitalWrite(10, HIGH); // Add this line
  
  //Inicia o cartao SD
  Serial.println("Iniciando cartao SD...");
  if (!SD.begin(Pino_CS))
  {
    Serial.println("Falha na inicializacao do SD!");
    return;
  }
}

void loop() {
  detachInterrupt(digitalPinToInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_PRX)));
  detachInterrupt(digitalPinToInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_ANT)));
  detachInterrupt(digitalPinToInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_REC)));
  Flash = !Flash;
  if(Flash)
    digitalWrite(LED_PIN, HIGH);
  else
    digitalWrite(LED_PIN, LOW);

  readSensorsData(); 
  
  if(grava) 
    grava_cartao_SD();
  
  display.clearDisplay();

  mostra_menu();
  mostra_label();
  mostra_info();
  
  display.display();      // Show initial tex
  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_PRX), opcao_mais, FALLING);
  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_ANT), opcao_menos, FALLING);
  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_REC), opcao_rec, FALLING);
}
