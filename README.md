# CarManGUI

### The Project
 
The project is being developed in partnership with FEB Racing([facebook](https://www.facebook.com/equipefebracing))([instagram](https://www.instagram.com/febracing/?hl=pt)), the formula SAE team from Unesp, for the construction of a device for the team's car.


![First test on the car](static%2Fprimeiro_teste_no_carro.jpeg)


In summary, it consists of:

1. Sensors placed on the car.
2. Recording of data collected from the sensors into a file for later analysis.
3. Displaying the data to the driver through a user interface and, in the future, to the team in the pit with the possibility of simple communication between the team and the driver:
   
    i. For example, showing the driver that a flag of color x has been given.
4. A plotter for data visualization and analysis.


### The Hardware
   
The current hardware of the project consists of:

1. 1 [Arduino Mega](https://www.arduino.cc/en/Guide/ArduinoMega2560)
2. Several sensors, including:

   i. 3 [MPU6050](https://www.letscontrolit.com/wiki/index.php/MPU6050) accelerometers and gyroscopes

   ii. 4 [MLX90614](https://forum.arduino.cc/index.php?topic=577921.0) infrared temperature sensors

   iii. 4 [KY003](https://www.instructables.com/Arduino-Magnetic-FIELD-Detector-Using-the-KY-003-o/) hall effect sensors

   iv. 4 potentiometers for suspension travel verification
   
    v. 1 brake line pressure sensor

3. 1 [0.96" OLED display](https://randomnerdtutorials.com/guide-for-oled-display-with-arduino/) that communicates via I2C
4. 3 buttons connected with a common pull-up
5. 1 [Raspberry Pi](https://circuitdigest.com/simple-raspberry-pi-projects-for-beginners)
6. 1 [3.5" touch displa](https://www.youtube.com/watch?v=Fj3wq98pd20), mounted on top of the Raspberry Pi, enabled via Raspbian
7. 1 4052 multiplexer
8. 1 [MCP2515](https://www.electronicshub.org/arduino-mcp2515-can-bus-tutorial/) adapter
 
  
In addition, the following are being implemented:
  - 4 [ATtiny85](https://thewanderingengineer.com/2014/02/17/attiny-i2c-slave/) microcontrollers.


### The Software

For the project implementation, C# is being used for the user interface, using a .NET Framework 4 project and [Mono].(https://www.mono-project.com/docs/getting-started/install/linux/#debian-ubuntu-and-derivatives)In the future, the Python will be used, which is compatible with Raspbian.

![teste_interface_csharpv2.x_animado](static%2Fteste_interface_csharpv2.x_animado.gif)

__The GIF shows a test of the C# window, with a 2D graph implemented, printing the values of 2 out of 3 accelerometer axes as data points while I sway the sensor.__

The project also utilizes Python, R, and C++ (Arduino, Processing, and an attempt to script for serial communication).


### Current Status


_-_ The project currently has a C# window developed to interface with the user through the Raspberry Pi's 3.5" display. It handles the initialization and termination of communication with the Arduino, controls when data recording starts, provides feedback when recording is in progress, and allows the recording to be stopped at any time*. It also displays sensor data on separate screens for better visualization.

_-_ The C# window did not function correctly on the Raspberry Pi after changing from manually using a thread to utilizing serial port events in C#. From my findings, the compatibility software Mono that I am using on the Raspberry Pi does not have the serial port event compatibility implemented.

_-_ Since the C# window did not work correctly on the Raspberry Pi, a 0.96" OLED display is being used to show desired information to the driver. It includes 2 buttons for menu selection, one for moving forward in the options and another for going back. The third button controls when the Arduino should start or stop recording. A SD card shield was supposed to be used, but it also did not work.

_-_ A Python window was developed to enable taking the functioning equipment to the competition since the SD module did not work directly connected to the Arduino. With this window, the Arduino can establish a connection and generate the log file. Due to the pandemic, it has not been possible to test the equipment on the team's car, but I will test it on my Corsa and post the results here. I will also test before and after installing the sway bar, as at that time, entry-level cars came so barebones that they didn't even come with safety features like the sway bar or the engine protection shield to reduce costs.

_-_ A test bench was created to facilitate the current bench tests, as seen in the following two images:


![Front view of the test bench](static%2Fbase_teste_v1.1_view2.jpeg)

![Top view of the test bench](static%2Fbase_teste_v1.1_view6.jpeg)


_-_ A flag alert feature was implemented to inform the driver of any conditions that require attention. This implementation allows for other necessary alerts to be conveyed to the driver. The image below shows how the accelerometer information was displayed:


![Flag alert test](static%2Fteste_alerta_bandeiras.gif)


_-_ The interface for accelerometer data has been improved to be more comprehensible for the driver. The lateral and forward-backward acceleration axes were chosen for implementation in how the point should move. I took inspiration from the G-force display in Gran Turismo. The following image shows the first version:

![First version of the accelerometer UI](static%2Fteste_UI_acelerometro_1.0.gif)


_-_ In the next image, the second version of the interface can be seen:

 
![Second version of the accelerometer UI](static%2Fteste_UI_acelerometro_1.1.gif)


### Planned Implementations

_-_ I plan to use Johnny-Five with Node.js to communicate between the Raspberry Pi and Arduino. I am also studying the possibility of creating an interface with React and preparing it for accessing the car via the internet on a computer in the pit using a React web page or even a mobile app with React Native.

_-_ As mentioned in the software section, before implementing with JS([JonhyFive](http://johnny-five.io/),[Node](https://www.instructables.com/NodeJs-and-Arduino/),[React](https://awot.net/en/guide/tutorial.html),ReactNative), I will likely implement a version using Python, which guarantees compatibility with Raspbian.

_-_ The ATtiny85 microcontrollers will be used as speed sensors, placed between the Arduino and the hall effect sensor, to correctly count pulses per time interval. If we were to directly use the 4 hall effect sensors on the Arduino, [the response would be poor](https://forum.arduino.cc/index.php?topic=519300.0) due to how wheel speed is calculated by sampling pulses per time. This is because it utilizes interrupt routines of the processor for such tasks. The idea is to have the ATtiny85 connected as a [slave on the I2C bus](https://thewanderingengineer.com/2014/02/17/attiny-i2c-slave/), and when the Arduino requests it, the ATtiny85 will send the current wheel speed, performing the interrupt calculations directly on the ATtiny85, which supports this. 

_-_ I am considering migrating the project directly to an [ESP8266](https://github.com/esp8266/Arduino) to not only utilize WiFi functionality in conjunction with the Raspberry Pi but also to improve the speed of the hardware responsible for capturing sensor data. This would allow for better sampling of sensor data and the possibility of adding more data capture, such as a CAN adapter and additional sensors.

_-_ I want to include reading the CAN bus in the equipment. I am currently trying to make it work with the [MCP2515](https://www.electronicshub.org/arduino-mcp2515-can-bus-tutorial/) adapter, but so far, I have been unable to initialize the adapter to proceed with reading the data from the engine control unit (ECU) to associate it with the sensor data log added by the project.
