# Related to threading tool buildup
import threading
import serial

class TkRepeatingTask():

    def __init__( self, tkRoot, taskFuncPointer, freqencyMillis ):
        self.__tk_   = tkRoot
        self.__func_ = taskFuncPointer        
        self.__freq_ = freqencyMillis
        self.__isRunning_ = False

    def isRunning( self ) : return self.__isRunning_ 

    def start( self ) : 
        self.__isRunning_ = True
        self.__onTimer()

    def stop( self ) : self.__isRunning_ = False

    def __onTimer( self ): 
        if self.__isRunning_ :
            self.__func_() 
            self.__tk_.after( self.__freq_, self.__onTimer )

class BackgroundTask():

    def __init__( self, taskFuncPointer ):
        self.__taskFuncPointer_ = taskFuncPointer
        self.__workerThread_ = None
        self.__isRunning_ = False

    def taskFuncPointer( self ) : return self.__taskFuncPointer_

    def isRunning( self ) : 
        return self.__isRunning_ and self.__workerThread_.isAlive()

    def start( self ): 
        if not self.__isRunning_ :
            self.__isRunning_ = True
            self.__workerThread_ = self.WorkerThread( self )
            self.__workerThread_.start()

    def stop( self ) : self.__isRunning_ = False

    class WorkerThread( threading.Thread ):
        def __init__( self, bgTask ):      
            threading.Thread.__init__( self )
            self.__bgTask_ = bgTask

        def run( self ):
            try :
                self.__bgTask_.taskFuncPointer()( self.__bgTask_.isRunning )
            except Exception as e: 
                print (e)
            self.__bgTask_.stop()
# Related to threading tool buildup

def tkThreadingTest():
    from tkinter import Tk, Label, Button, StringVar # changed the * to 'Tk, Label, Button, StringVar' because it may limit memory usage
    import tkinter.font
    import sys
    from time import sleep

    class UnitTestGUI:
        #variable related to serial port
        ser = None # = serial.Serial('COM4', 9600)#('/dev/ttyACM0', 9600) segundo valor é a iniciação da comunicação serial no raspberry
            
        def __init__( self, master ):
            self.master = master
            master.title( "Threading Test" )
            master.geometry('420x320')

             ##serial releated
            self.runComunication = 0

            self.serialButton = Button( 
                self.master, text="Conect Serial", command=self.onSerialClicked )
            self.serialButton.place(x = 10, y = 226)

            self.arduinoButton = Button( 
                self.master, text="Conect Arduino", command=self.onConectedClicked )
            self.arduinoButton.place(x = 90, y = 226)

            self.recButton = Button( 
                self.master, text="Rec", command=self.onRecClicked )
            self.recButton.place(x = 185, y = 226)

            self.cancelButton = Button( 
                self.master, text="Stop", command=self.onStopClicked )
            self.cancelButton.place(x = 240, y = 226)

            self.receviedInfo = StringVar()
            self.infoLabel = Label( master, textvariable=self.receviedInfo) 
            self.receviedInfo.set("Waiting connection...")
            self.infoLabel.place( x = 20, y = 20)

            self.bgTaskSerial = BackgroundTask( self.serialStartConection ) #BackgroundTask( self.myLongProcess )
            self.bgTaskArduino = BackgroundTask( self.serialConectionRead )
            self.bgTaskRecArduino = BackgroundTask( self.serialArduinoRec )
            
        def close( self ) :
            print ("close")
            try: self.bgTaskSerial.stop()
            except: pass
            try: self.bgTaskArduino.stop()
            except: pass
            try: self.bgTaskRecArduino.stop()
            except: pass            
            self.master.quit()

        def onSerialClicked( self ):
            print ("onSerialClicked")
            try: 
                if self.ser == None or self.ser :
                    self.bgTaskSerial.start()
            except: pass

        def onConectedClicked( self ):
            print ("onConectedClicked")
            try: self.bgTaskArduino.start()
            except: pass
            
        def onRecClicked( self ):
            print ("onRecClicked")
            try: self.bgTaskRecArduino.start()
            except: pass        

        def onStopClicked( self ) :
            print ("onStopClicked")
            try: self.bgTaskSerial.stop()
            except: pass
            try: self.bgTaskArduino.stop()
            except: pass
            try: self.bgTaskRecArduino.stop()
            except: pass 

        def serialStartConection ( self, isRunningFunc = None ) : #myLongProcess( self, isRunningFunc=None ) : using the long process as the serial thread handler
            print ("Starting Serial connection...")
            try :
                self.ser = serial.Serial('COM4', 9600) #('/dev/ttyACM0', 9600) 
                if(self.ser.isOpen() == False):
                    self.ser.open()     
            except :
                print("Error serial conection")        

        def serialConectionRead ( self, isRunningFunc = None  ) :
            self.keepGoing = 1
            self.ser.write(str.encode("1#"))

            while self.keepGoing == 1 :
                try:
                    if isRunningFunc() :
                        self.onSerialThreadUpdate(self.ser.readline())
                    else :
                        self.onSerialThreadUpdate ( "Serial connection stopped..." )
                        self.ser.write(str.encode("0#"))
                        self.keepGoing = 0
                except :
                    self.keepGoing = 0 
                
            self.onSerialThreadUpdate( "Serial connection stopped" ) 

        def serialArduinoRec ( self, isRunningFunc = None ) :
            try: 
                if isRunningFunc() :
                    self.onSerialThreadUpdate ( "Sending request to start/stops rec..." )
                    print("Sending request to start/stops rec...")
                    self.ser.write(str.encode("2#"))
            except : pass        

        def onSerialThreadUpdate( self, status ) : #onMyLongProcessUpdate( self, status ) :
            print( str(status) ) #print ("Process Update: %s" % (status))
            self.receviedInfo.set( str(status) ) #self.statusLabelVar.set( str.encode(status) )
            self.infoLabel.place( x = 20, y = 20)
            
    root = Tk()    
    gui = UnitTestGUI( root )
    root.protocol( "WM_DELETE_WINDOW", gui.close )
    root.mainloop()

if __name__ == "__main__": 
    tkThreadingTest()



# def conect_arduino():
#   global runComunication
#   global receviedInfo
  
#   if runComunication == 0 :
#     runComunication = 1
#     #receviedInfo.set("1")
#     #infoLabel.place( x = 20, y = 20 )
#   else:
#     runComunication = 0
#     #receviedInfo.set("0")
#     #infoLabel.place( x = 20, y = 20 )


# def rec_log():
#   global runComunication
#   global receviedInfo
  
#   if runComunication == 0 :
#     print("Sendig single way handshake to Arduino")
#     ser.write("2#")
#     runComunication = 1
#   else
#     ser.write("2#")  
#     runComunication = 0

# def readser():
#   global runComunication
#   global receviedInfo
  
#   if runComunication == 1 :
#     receviedInfo.set(ser.readline())
#     infoLabel.place( x = 20, y = 20 )
#     after(1,readser)
#   else :
#     receviedInfo.set("Not Conected")
#     infoLabel.place( x = 20, y = 20 )
    
# def exitProgram():
#     sys.exit()  