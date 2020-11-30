#include <SPI.h>
#include <Wire.h>
#include <Adafruit_MLX90614.h>
#include <SD.h>
#include "RTClib.h"

///
#define SERIAL_BAUDRATE 9600 
#define LED_PIN 13 
///MPU Releated
const short MPU[3] = {0x68, 0x69, 0x68};
//MLX releated
Adafruit_MLX90614 mlx;

//MLX
double  Temp[4] = { 8.1, 9.2, 10.3, 11.4 }; // temp dos discos de freio
//sensor de pressao
#define PIN_PRESS A0
int16_t Pressao = 12; // pressao do freio     

short sensor_1 = 0, sensor_2 = 1, sensor_3 = 2, sensor_4 = 3;
  
#define INTERRUPT_PIN 2 //pinos que funcionam: 2, 3, 19, 18 no mega, no uno 2 e 3 somente

//Pino CS do cartao SD
int Pino_CS = 10;
File file; // variável de arquivo
RTC_DS1307 rtc; // módulo rtc com o real time
bool grava = false; // variável que indica se vai gravar ou não

void getTempValue(int pMultiplex, double *pTemp)
{
  *pTemp = mlx.readObjectTempC();
}

void getPressValue(int pMultiplex, int16_t *pPress)
{
  *pPress = analogRead(PIN_PRESS);
}

void readSensorsData(){
  getTempValue      (sensor_1,
                     &Temp[sensor_1]);
  getPressValue     (sensor_1,
                     &Pressao);                   
  Serial.print(Temp[sensor_1]);  
  Serial.print(Pressao);                 
}

///

void grava_SD ()
{
  detachInterrupt(digitalPinToInterrupt(digitalPinToInterrupt(INTERRUPT_PIN)));
  
   static unsigned long last_interrupt_time = 0;
   unsigned long interrupt_time = millis();
   // If interrupts come faster than 200ms, assume it's a bounce and ignore
   if (interrupt_time - last_interrupt_time > 200)
   {
     grava = !grava;
   }
   last_interrupt_time = interrupt_time;
  
  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN), grava_SD, FALLING);
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
  file.print(Temp[sensor_1]);file.print(':');

  file.println(Pressao);
  //Fecha arquivo
  file.close();
}
///

void setup() {
  pinMode(LED_PIN, OUTPUT);
  Serial.begin(SERIAL_BAUDRATE);

  Wire.begin(); 

  Serial.println("Inicializando MLX...");
  mlx = Adafruit_MLX90614();
  
  if(!mlx.begin())
    Serial.println("MLX não inicializado"); 

  Serial.println("MLX inicializado");
  Serial.println("Iniciando cartao SD...");
  
  if (!SD.begin(Pino_CS))
  {
    Serial.println("Falha na inicializacao do SD!");
    return;
  }
}

void loop() {
  detachInterrupt(digitalPinToInterrupt(digitalPinToInterrupt(INTERRUPT_PIN)));
  
  if(grava)
    digitalWrite(LED_PIN, HIGH);
  else
    digitalWrite(LED_PIN, LOW);

  readSensorsData(); 
  
  if(grava) 
    grava_cartao_SD();

  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN), grava_SD, FALLING);
}
