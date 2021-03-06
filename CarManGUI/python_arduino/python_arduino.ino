#include <mcp_can.h>
#include <mcp_can_dfs.h>
#include <SPI.h>
#include <Wire.h>
#include <MPU6050.h>

// ## CAN related ------------------------------------------------------

// //const int CANoutput = 21,
// //int data;
 int time = 0;
 int i = 20;

// //CAN receive related variables -------------------------------
 long unsigned int rxId;
 unsigned char len = 0;
 unsigned char rxBuf[8];
 char msgString[128];                        // Array to store serial string

// //CAN send related variables ----------------------------------
 byte data[8] = {0x00, 0x02, 0x09, 0x03, 0x04, 0x05, 0x06, 0x07};

// //CAN related setup ------------------------------------------
// #define CAN0_INT 2                              // Set INT to pin 2
// MCP_CAN CAN0(9);                               // Set CS to pin 9

// ## CAN related ------------------------------------------------------

// ##
#define LED_PIN 13                      //Pin number on which the LED is connected
#define LED_TURN_ON_TIMEOUT  1000       //Timeout for LED power time (defines how long the LED stays powered on) in milliseconds
#define SERIAL_BAUDRATE 9600            //Baud-Rate of the serial Port // 115200
// ##

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

// ## MPU-6050 (Giroscópio/Acelerômetro) -------------------------------

//Endereco I2C do MPU6050
const int MPU1 = 0x68;  //MPU com AD0 no GND ou sem nada
const int MPU2 = 0x69;  //MPU com AD0 no GND ou sem nada
const int MPU3 = 0x60;  //MPU com AD0 no 5V

//MPU6050 mpu; //variável MPU da biblioteca MPU
//Variaveis para armazenar os valores dos sensores
//int AcX, AcY, AcZ, GyX, GyY, GyZ;
int16_t AcX1 = 0, AcY1 = 0, AcZ1 = 0, GyX1 = 0, GyY1 = 0, GyZ1 = 0; //MPU 1
int16_t AcX2 = 1, AcY2 = 2, AcZ2 = 3, GyX2 = 4, GyY2 = 5, GyZ2 = 6; //MPU 2
int16_t AcX3 = 1, AcY3 = 2, AcZ3 = 3, GyX3 = 4, GyY3 = 5, GyZ3 = 6; //MPU 3
//float x_acc, gyro, AX = 0.0, AY = 0.0, AZ = 0.0, GX = 0.0, GY = 0.0, GZ = 0.0;      
// ## MPU-6050 (Giroscópio/Acelerômetro) -------------------------------

// ## MLX 90614 (temp discos)
//Endereco I2C do MLX 90614
const int MLX = 0x5A;
//variáveis para armazenar os valores dos sensores
int16_t Temp1 = 180, Temp2 = 200, Temp3 = 120, Temp4 = 150;
// ## MLX 90614 (temp discos)

// ## Potenciometros da suspensão
//variáveis para armazenar os valores dos sensores
int16_t Posic1 = 2, Posic2 = 52, Posic3 = 5, Posic4 = 43, BrakePress = 10;
//&, 
int AnalogPin5 = A11, AnalogPin1 = A12, AnalogPin2 = A13, AnalogPin3 = A14, AnalogPin4 = A15;
//## Potenciometros da suspensão

// ## KY003(velocidade das rodas)
//variáveis para armazenasr os valores dos sensores
int16_t VelR1 = 178, VelR2 = 180, VelR3 = 178, VelR4 = 180;
//## KY003(velocidade das rodas)

// ## Multiplex
//variáveis para selecionar os pinos que vão controlar os sensores selecionados no multiplexador
int MultiplexPinA = 22, MultiplexPinB = 23;
// ## Multiplex

void setup() 
{// put your setup code here, to run once:
  pinMode(LED_PIN, OUTPUT);
  Serial.begin(SERIAL_BAUDRATE);

  // ## Multiplexador
  pinMode(MultiplexPinA, OUTPUT);
  pinMode(MultiplexPinB, OUTPUT);

  // ## Multiplexador

// ## CAN DATA SETUP -------------------------------------------------------------------------------------------
/*
   // Initialize MCP2515 running at 16MHz with a baudrate of 500kb/s and the masks and filters disabled.
   if(CAN0.begin(MCP_ANY, CAN_125KBPS, MCP_8MHZ) == CAN_OK) //MCP_16MHZ
     Serial.println("MCP2515 Initialized Successfully!");
   else
     Serial.println("Error Initializing MCP2515...");
  
   CAN0.setMode(MCP_NORMAL);                     // Set operation mode to normal so the MCP2515 sends acks to received data.

   pinMode(CAN0_INT, INPUT);                            // Configuring pin for /INT input
 */ 
  // Serial.println("MCP2515 Library Receive Example...");

  // //------ sending CAN data
  // // send data:  ID = 0x100, Standard CAN Frame, Data length = 8 bytes, 'data' = array of data bytes to send
  // //CAN ID to services 01
  // byte sndStat = CAN0.sendMsgBuf(0x100, 0, 8, data);
  // //byte sndStat = CAN0.sendMsgBuf(0x001, 0, 8, data);
  
  // if(sndStat == CAN_OK)
  //   Serial.println("Message Sent Successfully!");
  // else
  //   Serial.println("Error Sending Message...");
  
  // //delay(100);   // send data per 100ms

// ## CAN DATA SETUP -------------------------------------------------------------------------------------------

// ## MPU 6050 (Giroscópio/Acelerômetro) ------------------------------------------
//ver 1.1 - utilizando a biblioteca Wire para inicializar o MPU
  Wire.begin(); 
  initializeMPU(1,MPU1);
  initializeMPU(2,MPU2);
  //initializeMPU(3,MPU3);
//ver 1.1 - ==
//ver 1.0 - utilizando a biblioteca do sensor (mpu)  
//  mpu.initialize(); // utilizando a biblioteca MPU
//
//  mpu.setFullScaleAccelRange(0);  //0 = +/- 2g | 1 = +/- 4g | 2 = +/- 8g | 3 =  +/- 16g 
//  mpu.setFullScaleGyroRange(0); //0 = +/- 250 degrees/sec | 1 = +/- 500 degrees/sec | 2 = +/- 1000 degrees/sec | 3 =  +/- 2000 degrees/sec
//ver 1.0 - ==
// ## MPU 6050 (Giroscópio/Acelerômetro) ------------------------------------------ 

// ## MLX 90614 (Temperatura) ------------------------------------------ 
  initializeMLX(1);
  initializeMLX(2);
  initializeMLX(3);
  initializeMLX(4);
// ## MLX 90614 (Temperatura) ------------------------------------------ 
}

// ---------------------------
// ----- Funções criadas -----
// ---------------------------

void getGyroValues(int pMultiplex, int pMPU, int16_t *pGyX, int16_t *pGyY, int16_t *pGyZ) 
{ // MPU 6050 stuff
  if(pMultiplex == 1 || pMultiplex == 2) // se for um dos dois MPUs ligados juntos no primeiro grupo do multiplexador
  {
    digitalWrite(MultiplexPinA, LOW);
    digitalWrite(MultiplexPinB, LOW);
  }
  else // se não liga no segundo
  {
    digitalWrite(MultiplexPinA, HIGH);
    digitalWrite(MultiplexPinB, LOW);   
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
    digitalWrite(MultiplexPinA, LOW);
    digitalWrite(MultiplexPinB, LOW);
  }
  else // se não liga no segundo
  {
    digitalWrite(MultiplexPinA, HIGH);
    digitalWrite(MultiplexPinB, LOW);   
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

void getTempValue(int pMultiplex, int16_t *pTemp)
{
  switch(pMultiplex) // seleciona qual dos 4 está selecionado pelo multiplex
  {
    case 1:
            digitalWrite(MultiplexPinA, LOW);
            digitalWrite(MultiplexPinB, LOW);
            break;
    case 2:        
            digitalWrite(MultiplexPinA, HIGH);
            digitalWrite(MultiplexPinB, LOW);
            break;   
    case 3:
            digitalWrite(MultiplexPinA, LOW);
            digitalWrite(MultiplexPinB, HIGH);
            break;         
    case 4:
            digitalWrite(MultiplexPinA, HIGH);
            digitalWrite(MultiplexPinB, HIGH);
            break;         
  }  
  //
  /* Lecture des données : 1 mot sur 16 bits + octet de contrôle (PEC) */
  Wire.requestFrom(MLX, 3, false);
  while(Wire.available() < 3);
  *pTemp = Wire.read();
  *pTemp |= (Wire.read() & 0x7F) << 8;  // Le MSB est ignoré (bit de contrôle d'erreur)
  Wire.read(); // PEC
  Wire.endTransmission(); //talvez colocar false para não dar problema com o MLX
}

void getAnalogValue(int16_t *pPosic, int pAnalogPin)
{
  *pPosic = analogRead(pAnalogPin);
}

void initializeMPU(int pMultiplex, int pMPU)
{
  //Serial.print("Iniciando MPU");
  if(pMultiplex == 1 || pMultiplex == 2) // se for um dos dois MPUs ligados juntos no primeiro grupo do multiplexador
  {
    digitalWrite(MultiplexPinA, LOW);
    digitalWrite(MultiplexPinB, LOW);
  }
  else
  {
    digitalWrite(MultiplexPinA, HIGH);
    digitalWrite(MultiplexPinB, LOW);   
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

void initializeMLX(int pMultiplex)
{
  switch(pMultiplex) // seleciona qual dos 4 está selecionado pelo multiplex
  {
    case 1:
            digitalWrite(MultiplexPinA, LOW);
            digitalWrite(MultiplexPinB, LOW);
            break;
    case 2:        
            digitalWrite(MultiplexPinA, HIGH);
            digitalWrite(MultiplexPinB, LOW);
            break;   
    case 3:
            digitalWrite(MultiplexPinA, LOW);
            digitalWrite(MultiplexPinB, HIGH);
            break;         
    case 4:
            digitalWrite(MultiplexPinA, HIGH);
            digitalWrite(MultiplexPinB, HIGH);
            break;         
  }  
  ///* Commande de lecture de la RAM à l'adresse 0x07 */
  Wire.beginTransmission(MLX);
  Wire.write(0x07);
  Wire.endTransmission(false);  
}

// 1.4
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

void readSensorsData(){
  //ver 1.3 - agrupando a atualização de valores antes de começar a mandar pela serial
  getAccelValues    (1,MPU1, &AcX1, &AcY1, &AcZ1);
  getGyroValues     (1,MPU1, &GyX1, &GyY1, &GyZ1);
  getAccelValues    (2,MPU2, &AcX2, &AcY2, &AcZ2);
  getGyroValues     (2,MPU2, &GyX2, &GyY2, &GyZ2);
  //getAccelValues    (3,MPU3, &AcX3, &AcY3, &AcZ3);
  //getGyroValues     (3,MPU3, &GyX3, &GyY3, &GyZ3);
  getTempValue      (1,&Temp1);
  getTempValue      (2,&Temp2);
  getTempValue      (3,&Temp3);
  getTempValue      (4,&Temp4);
  //getAnalogValue (&Posic1,AnalogPin1); //gets suspension 1 posicion value
  //getAnalogValue (&Posic2,AnalogPin2); //gets suspension 2 posicion value
  //getAnalogValue (&Posic3,AnalogPin3); //gets suspension 3 posicion value
  //getAnalogValue (&Posic4,AnalogPin4); //gets suspension 4 posicion value
  //getAnalogValue (&BrakePress, AnalogPin5); //gets brake line pressure 
  //getVelRodaValue   (&VelR1);
  //getVelRodaValue   (&VelR2);
  //getVelRodaValue   (&VelR3);
  //getVelRodaValue   (&VelR4);
}

void sendSensorsData(){
  //ver 1.2 - printando todas as entradas dos sensores como inteiros na serial para fazer a conversão no Raspberry
  Serial.print("_");
  //MPU 1
  Serial.print(GyX1);//1
  Serial.print(":");
  Serial.print(GyY1);//2
  Serial.print(":");
  Serial.print(GyZ1);//3
  Serial.print(":");
  Serial.print(AcX1);//4
  Serial.print(":");
  Serial.print(AcY1);//5
  Serial.print(":");
  Serial.print(AcZ1);//6
  Serial.print(":");

  //MPU 2
  Serial.print(GyX2);//7
  Serial.print(":");
  Serial.print(GyY2);//8
  Serial.print(":");
  Serial.print(GyZ2);//9
  Serial.print(":");
  Serial.print(AcX2);//10
  Serial.print(":");
  Serial.print(AcY2);//11
  Serial.print(":");
  Serial.print(AcZ2);//12
  Serial.print(":");

  //MPU 3
  Serial.print(GyX3);//13
  Serial.print(":");
  Serial.print(GyY3);//14
  Serial.print(":");
  Serial.print(GyZ3);//15
  Serial.print(":");
  Serial.print(AcX3);//16
  Serial.print(":");
  Serial.print(AcY3);//17
  Serial.print(":");
  Serial.print(AcZ3);//18
  Serial.print(":");
  
  //MLXs (temp discos)
  Serial.print(Temp1);//19
  Serial.print(":");
  Serial.print(Temp2);//20
  Serial.print(":");
  Serial.print(Temp3);//21
  Serial.print(":");
  Serial.print(Temp4);//22
  Serial.print(":");
  
  //Potenciometros da suspensão
  Serial.print(Posic1);//23
  Serial.print(":");
  Serial.print(Posic2);//24
  Serial.print(":");
  Serial.print(Posic3);//25
  Serial.print(":");
  Serial.print(Posic4);//26
  Serial.print(":");
  
  //KY003 (velocidade das rodas)
  Serial.print(VelR1);//27
  Serial.print(":");
  Serial.print(VelR2);//28
  Serial.print(":");
  Serial.print(VelR3);//29
  Serial.print(":");
  Serial.print(VelR4);//30

  Serial.print("\n");
}

// ---------------------------
// -----\Funções criadas -----
// ---------------------------

void loop() 
{
  
// ## CAN Stuff --------------------------------------------------------------------

   //------ receiving CAN data and seding data through
   //data = analogRead(CANInput); depois colocar aqui por onde o arduino
   //                             receberá os dados do conversor CAN
/*   if(!digitalRead(CAN0_INT))                         // If CAN0_INT pin is low, read receive buffer
   {
     CAN0.readMsgBuf(&rxId, &len, rxBuf);      // Read data: len = data length, buf = data byte(s)
    
     if((rxId & 0x80000000) == 0x80000000)     // Determine if ID is standard (11 bits) or extended (29 bits)
       sprintf(msgString, "Extended ID: 0x%.8lX  DLC: %1d  Data:", (rxId & 0x1FFFFFFF), len);
     else
       sprintf(msgString, "Standard ID: 0x%.3lX       DLC: %1d  Data:", rxId, len);
  
     Serial.print(msgString);
  
     if((rxId & 0x40000000) == 0x40000000){    // Determine if message is a remote request frame.
       sprintf(msgString, " REMOTE REQUEST FRAME");
       Serial.print(msgString);
     } 
     else {
       for(byte i = 0; i<len; i++){
         sprintf(msgString, " 0x%.2X", rxBuf[i]);
         Serial.print(msgString);
       }
     }
    
     delay(100);
     Serial.flush();
   }
*/
// ## CAN Stuff --------------------------------------------------------------------

// ## Sensores ---------------------------------------------------------------------
  

  //readSensorsData();
  //sendSensorsData(); 
  String command = "";  //Used to store the latest received command
  int serialResult = 0; //return value for reading operation method on serial in put buffer

  //Serial.print("Iniciado");
  
  serialResult = readSerialInputCommand(&command);
  if(serialResult == MSG_METHOD_SUCCESS){
    if(command == "1#"){//Request for sending data via Serial Interface
        didConect = 1;
        readSensorsData();
        sendSensorsData();
        //ver 1.2 - ==
        //Aguarda 300 ms e reinicia o processo  
        delay(300);
    }
    if(command == "0#")
      didConect = 0;
  }

  if(serialResult == WRG_NO_SERIAL_DATA_AVAILABLE && didConect == 1){//){//If there is no data AVAILABLE at the serial port, let the LED blink
     readSensorsData();
     sendSensorsData();
     //ver 1.2 - ==
     //Aguarda 300 ms e reinicia o processo  
     delay(300);
  }
  else{
    if((serialResult == WRG_NO_SERIAL_DATA_AVAILABLE) && didConect != 1){//If there is no data AVAILABLE at the serial port, let the LED blink
     digitalWrite(LED_PIN, HIGH);
     delay(LED_TURN_ON_TIMEOUT);
     digitalWrite(LED_PIN, LOW);
     delay(LED_TURN_ON_TIMEOUT);
    }
    if(serialResult == ERR_SERIAL_IN_COMMAND_NOT_TERMINATED){//If the command format was invalid, the led is turned off for two seconds
      digitalWrite(LED_PIN, LOW);
      delay(300);
    }
  }

// ## Sensores ---------------------------------------------------------------------

}
