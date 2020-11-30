#include <SoftwareSerial.h>

SoftwareSerial serial_gps(7,6);//RX, TX

void setup() {
  // put your setup code here, to run once:
  serial_gps.begin(9600);
  Serial.begin(9600);
  Serial.println("Connecting...");
}

void loop() {
  // put your main code here, to run repeatedly:
  serial_gps.listen();
  while(serial_gps.available())
  {
    char c = serial_gps.read();
    Serial.print(c);
  }
}
