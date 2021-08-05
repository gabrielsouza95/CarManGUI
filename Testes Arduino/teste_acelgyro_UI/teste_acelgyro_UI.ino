#include <SPI.h>
#include <Wire.h>
#include <MPU6050.h>
#include <Adafruit_GFX.h> //OLED
#include <Adafruit_SSD1306.h> //OLED
#include <SD.h>
#include "RTClib.h"
#include <math.h>

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
#define CONT_OFFSET   8

#define COL1_BOX_ACEL  0
#define COL2_BOX_ACEL  120
#define LIN1_BOX_ACEL  9
#define LIN2_BOX_ACEL  57 //Last value that still appear on screen
#define COL_ACEL_INFO_OFFSET 20
///Opcao selecionada
short opcao = 0;

// ## Serial status ---------------------------------------------------

int didConect = 0; //keeps the value if the connection was made
 /*   WARNING, ERROR AND STATUS CODES                              */
//STATUS
#define MSG_METHOD_SUCCESS 0                      //Code which is used when an operation terminated  successfully
//WARNINGS
#define WRG_NO_SERIAL_DATA_AVAILABLE 250          //Code indicates that no new data is available at the serial input buffer
//ERRORS
#define ERR_SERIAL_IN_COMMAND_NOT_TERMINATED -1   //Code is used when a serial input commands' last char is not a '#' 

// ## Serial status ---------------------------------------------------

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

  //escreve se está conectado a serial
  display.setTextSize(1); // Draw 2X-scale text
  display.setTextColor(WHITE);
  display.setCursor(COL_MENU1, LIN_MENU + CONT_OFFSET);

  if(didConect == 1)
    display.print("CON:Y");
  else
    display.print("CON:N");

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
    case  2:break;//display.print("A.X:");
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
    case  2:break;//display.print("A.Y:");
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
    case  2:label_acel();break;//display.print("A.Z:");
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

void label_acel()
{
  //label espaço ponto
  short lin_lat = 1;
  display.setTextSize(1); 
  display.setCursor(COL1_BOX_ACEL - COL_ACEL_INFO_OFFSET, LIN1_BOX_ACEL);
  display.print("      ________      ");//20 é a quantidade de caracteres no máximo, nesse tamanho, que cabem na tela na horizontal.
  for (int ite = 0; ite < 8; ite ++)
  {
    display.setCursor(COL1_BOX_ACEL - COL_ACEL_INFO_OFFSET, LIN1_BOX_ACEL+6*lin_lat++);
    display.print("     |        |    ");//20  
  }
  display.setCursor(COL1_BOX_ACEL - COL_ACEL_INFO_OFFSET, LIN2_BOX_ACEL); 
  display.print("      ________      ");//"_________||_________"
  //label espaço ponto
  //label valor acel
  display.setTextSize(2);
  display.setCursor( (COL1_BOX_ACEL + 90) - COL_ACEL_INFO_OFFSET, LIN1_BOX_ACEL+35);
  display.print("Gs"); 
  //label valor acel
}

void ponto_acel(float pX, float pY)
{
  display.setTextSize(1);
  //coluna - min = 0;  max = 114 
  //linha  - min = 10; max = 57
  display.setCursor( ( (114+(33*pX))/2 ) - COL_ACEL_INFO_OFFSET, (66+(33*pY))/2 );
  display.print(".");
  display.setCursor( ( (114+(33*pX))/2 ) - COL_ACEL_INFO_OFFSET + 1, (66+(33*pY))/2 + 3);
  display.print("+");  
}

void valor_acel(float pX, float pY)
{
  display.setTextSize(2);
  display.setCursor( (COL1_BOX_ACEL + 90) - COL_ACEL_INFO_OFFSET, LIN1_BOX_ACEL+15);
  display.print(sqrt((pX*pX)+pY*pY));  
}

void mostra_info()
{
  display.setTextSize(2); // Draw 2X-scale text
  
  ///infos
  display.setCursor(COL_INFO, LIN_INFO1);
  float AcelConv = 0.0000;
  switch(opcao)///opcao atual
  {
    case  0:break;//AcelConv = (AcX[sensor_1]/Acel_convert);display.print(AcelConv);break;
    case  1:break;//AcelConv = ((float)AcX[sensor_2]/(float)Acel_convert);display.print(AcelConv);break;
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
    case  0:break;//AcelConv = (AcY[sensor_1]/Acel_convert);display.print(AcelConv);break;
    case  1:break;//AcelConv = ((float)AcY[sensor_2]/(float)Acel_convert);display.print(AcelConv);break;
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
    case  0:ponto_acel( ((float)AcX[sensor_1]/(float)Acel_convert),((float)AcY[sensor_1]/(float)Acel_convert) );
            valor_acel( ((float)AcY[sensor_1]/(float)Acel_convert)*-1,((float)AcX[sensor_1]/(float)Acel_convert) );break;//AcelConv = (AcZ[sensor_1]/Acel_convert);display.print(AcelConv);break;
    case  1:ponto_acel( ((float)AcY[sensor_2]/(float)Acel_convert)*-1,((float)AcX[sensor_2]/(float)Acel_convert) );
            valor_acel( ((float)AcY[sensor_2]/(float)Acel_convert)*-1,((float)AcX[sensor_2]/(float)Acel_convert) ); break;//AcelConv = ((float)AcZ[sensor_2]/(float)Acel_convert);display.print(AcelConv);break;
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

/// s@han1maia
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
 
  file.println(Susp[sensor_1]);file.print(':');
  file.println(Susp[sensor_2]);file.print(':');
  file.println(Susp[sensor_3]);file.print(':');
  file.println(Susp[sensor_4]);
  
  file.println(Pressao);file.print(':');
  file.println(VelRoda);file.print(':');
  //Fecha arquivo
  file.close();
}
///

void sendSensorsData(){
  //ver 1.2 - printando todas as entradas dos sensores como inteiros na serial para fazer a conversão no Raspberry
  Serial.print("_");
  //Indica se está gravando para o rasp
  Serial.print(grava);Serial.print(":");//0
  
  //MPU 1
  Serial.print(GyX[sensor_1]);Serial.print(":");//1
  Serial.print(GyY[sensor_1]);Serial.print(":");//2
  Serial.print(GyZ[sensor_1]);Serial.print(":");//3
  Serial.print(AcX[sensor_1]);Serial.print(":");//4 
  Serial.print(AcY[sensor_1]);Serial.print(":");//5 
  Serial.print(AcZ[sensor_1]);Serial.print(":");//6 

  //MPU 2
  Serial.print(GyX[sensor_2]);Serial.print(":");//7
  Serial.print(GyY[sensor_2]);Serial.print(":");//8
  Serial.print(GyZ[sensor_2]);Serial.print(":");//9
  Serial.print(AcX[sensor_2]);Serial.print(":");//10
  Serial.print(AcY[sensor_2]);Serial.print(":");//11
  Serial.print(AcZ[sensor_2]);Serial.print(":");//12
  

  //MPU 3
  Serial.print(GyX[sensor_3]);Serial.print(":");//13
  Serial.print(GyY[sensor_3]);Serial.print(":");//14
  Serial.print(GyZ[sensor_3]);Serial.print(":");//15
  Serial.print(AcX[sensor_3]);Serial.print(":");//16
  Serial.print(AcY[sensor_3]);Serial.print(":");//17
  Serial.print(AcZ[sensor_3]);Serial.print(":");//18
  
  
  //MLXs (temp discos)
  Serial.print(Temp[sensor_1]);Serial.print(":");//19
  Serial.print(Temp[sensor_2]);Serial.print(":");//20 
  Serial.print(Temp[sensor_3]);Serial.print(":");//21
  Serial.print(Temp[sensor_4]);Serial.print(":");//22
  
  
  //Potenciometros da suspensão
  Serial.print(Susp[sensor_1]);Serial.print(":");//23
  Serial.print(Susp[sensor_2]);Serial.print(":");//24
  Serial.print(Susp[sensor_3]);Serial.print(":");//25
  Serial.print(Susp[sensor_4]);Serial.print(":");//26
 
  //Pressão linha do freio  
  Serial.print(Pressao);Serial.print(":");//27 
  //KY003 (velocidade das rodas)
  Serial.print(VelRoda);//28

  Serial.print("\n");
}

// Serial comunication
int readSerialInputCommand(String *command){
  
  int operationStatus = MSG_METHOD_SUCCESS;//Default return is MSG_METHOD_SUCCESS reading data from com buffer.

  //check if serial data is available for reading
  if (Serial.available()) {
     char serialInByte;//temporary variable to hold the last serial input buffer character

     do{//Read serial input buffer data byte by byte 
       serialInByte = Serial.read();
       *command = *command + serialInByte;//Add last read serial input buffer byte to *command pointer
     }while(serialInByte != '#' && Serial.available());//until '#' comes up or no serial data is available anymore
     

     if(serialInByte != '#') {
       operationStatus = ERR_SERIAL_IN_COMMAND_NOT_TERMINATED;
     }
  }
  else{//If not serial input buffer data is AVAILABLE, operationStatus becomes WRG_NO_SERIAL_DATA_AVAILABLE (= No data in the serial input buffer AVAILABLE)
    operationStatus = WRG_NO_SERIAL_DATA_AVAILABLE;
  }
  
  return operationStatus;
}

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
  
//
  readSensorsData();
  String command = "";  //Used to store the latest received command
  int serialResult = 0; //return value for reading operation method on serial in put buffer
  serialResult = readSerialInputCommand(&command);
  
  if(serialResult == MSG_METHOD_SUCCESS){
    if(command == "1#"){//Request for sending data via Serial Interface
        didConect = 1;
        sendSensorsData();
        //ver 1.2 - ==
        //Aguarda 300 ms e reinicia o processo  
    }
    if(command == "0#") //Request to stop comunication
    {
      didConect = 0;
      if(grava)
        grava = false;
    }
    if(command == "2#") //Request to start or stop serial comunication
    {
      if(!grava && didConect == 1)
        grava = true;
      else
        grava = false;  
    }   
  }

  if(serialResult == WRG_NO_SERIAL_DATA_AVAILABLE && didConect == 1){//If there is no data AVAILABLE at the serial port, but conected, keep sending information
     sendSensorsData();
     //ver 1.2 - ==
     //Aguarda 300 ms e reinicia o processo  
  }
  else{
    if((serialResult == WRG_NO_SERIAL_DATA_AVAILABLE) && didConect != 1){//If there is no data AVAILABLE at the serial port, let the LED blink
      Flash = !Flash;
      if(Flash)
        digitalWrite(LED_PIN, HIGH);
      else
        digitalWrite(LED_PIN, LOW);
    }
    if(serialResult == ERR_SERIAL_IN_COMMAND_NOT_TERMINATED){//If the command format was invalid, the led is turned off for two seconds
      digitalWrite(LED_PIN, LOW);
    }
  }
//  
  //if(grava) 
  //  grava_cartao_SD();
  
  display.clearDisplay();

  mostra_menu();
  mostra_label();
  mostra_info();
  
  display.display();      // Show initial tex
  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_PRX), opcao_mais, FALLING);
  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_ANT), opcao_menos, FALLING);
  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN_REC), opcao_rec, FALLING);
}
