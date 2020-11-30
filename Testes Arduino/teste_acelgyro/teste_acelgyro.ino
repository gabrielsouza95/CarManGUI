#include <Wire.h>
#include <MPU6050.h>

#define SERIAL_BAUDRATE 9600 
#define LED_PIN 13 

const int MPU[3] = {0x68, 0x69, 0x68};
int16_t AcX[3] = {0,0,0}, 
        AcY[3] = {0,0,0}, 
        AcZ[3] = {0,0,0}, 
        GyX[3] = {0,0,0}, 
        GyY[3] = {0,0,0}, 
        GyZ[3] = {0,0,0}; //MPU 
int MultiplexPin[2] = {22, 23}; //posição 1 = pino A
                                //posição 2 = pino B
bool Flash = false;

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

void sendSensorsData(){
  Serial.print("_");
  //MPU 1
  for(int i = 0; i < 3; i++){
    Serial.print(GyX[i]);//1
    Serial.print(":");
    Serial.print(GyY[i]);//2
    Serial.print(":");
    Serial.print(GyZ[i]);//3
    Serial.print(":");
    Serial.print(AcX[i]);//4
    Serial.print(":");
    Serial.print(AcY[i]);//5
    Serial.print(":");
    Serial.print(AcZ[i]);//6
    if(i != 2)
      Serial.print(":");
    else
      Serial.print("\n");
  }
}

void readSensorsData(){
  int sensor_1 = 0, sensor_2 = 1, sensor_3 = 2;
  
  getAccelValues    (sensor_1 + 1,
                     MPU[sensor_1], 
                     &AcX[sensor_1], 
                     &AcY[sensor_1], 
                     &AcZ[sensor_1]);
                     
  getGyroValues     (sensor_1 + 1,
                     MPU[sensor_1], 
                     &GyX[sensor_1], 
                     &GyY[sensor_1], 
                     &GyZ[sensor_1]);
  
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
  
  /*getAccelValues    (sensor_3 + 1,
                       MPU[sensor_3], 
                       &AcX[sensor_3], 
                       &AcY[sensor_3], 
                       &AcZ[sensor_3]);*/
  /*getGyroValues     (sensor_3 + 1,
                       MPU[sensor_3], 
                       &GyX[sensor_3], 
                       &GyY[sensor_3], 
                       &GyZ[sensor_3]);*/
}

void setup() {
  // put your setup code here, to run once:
  int sensor_1 = 0, sensor_2 = 1, sensor_3 = 2;
  pinMode(LED_PIN, OUTPUT);
  Serial.begin(SERIAL_BAUDRATE);
  
  pinMode(MultiplexPin[0], OUTPUT);
  pinMode(MultiplexPin[1], OUTPUT);

  Wire.begin(); 
  
  initializeMPU(sensor_1 + 1, MPU[sensor_1]);
  initializeMPU(sensor_2 + 1, MPU[sensor_2]);
  //initializeMPU(sensor_3 + 1, MPU[sensor_3]);
}

void loop() {
  // put your main code here, to run repeatedly:
  Flash = !Flash;
  if(Flash)
    digitalWrite(LED_PIN, HIGH);
  else
    digitalWrite(LED_PIN, LOW);
  
  readSensorsData();
  sendSensorsData();                     
  delay(200);
}
