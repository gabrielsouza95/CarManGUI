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
        
        def __init__( self, master ):
            self.master = master
            self.master.title( "CarManGUI" )
            self.master.geometry('420x320')
            
            ##button position related
            self.buttonYLine = 285

            ##serial releated
            self.runComunication = 0
            #variable related to serial port
            self.ser = None # = serial.Serial('COM4', 9600)#('/dev/ttyACM0', 9600) segundo valor é a iniciação da comunicação serial no raspberry        

            ##file related
            self.saveFile = False # variável que vai indicar se tem que gravar o arquivo ou não quando executar a thread da serial
            self.logFile = None 

            self.serialButton = Button( 
                self.master, text="Conect Serial", command=self.onSerialClicked )
            self.serialButton.place(x = 10, y = self.buttonYLine)

            self.arduinoButton = Button( 
                self.master, text="Conect Arduino", command=self.onConectedClicked )
            self.arduinoButton.place(x = 90, y = self.buttonYLine)

            self.recButton = Button( 
                self.master, text="Rec", command=self.onRecClicked )
            self.recButton.place(x = 185, y = self.buttonYLine)

            self.cancelButton = Button( 
                self.master, text="Stop", command=self.onStopClicked )
            self.cancelButton.place(x = 240, y = self.buttonYLine)

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
                if self.ser == None : # or self.ser : #TO DO : não esquecer de checar problemas com a serial também além do None
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
            self.onSerialThreadUpdate ( "Starting Serial connection..." )
            try :
                self.ser = serial.Serial('COM4', 9600) #('/dev/ttyACM0', 9600) 
                if(self.ser.isOpen() == False):
                    self.ser.open()
                    self.onSerialThreadUpdate ( "Conected to Arduino..." )     
            except :
                self.onSerialThreadUpdate ( "Error serial conection" )     

        def serialConectionRead ( self, isRunningFunc = None  ) :
            self.keepGoing = 1
            self.ser.write(str.encode("1#"))

            while self.keepGoing == 1 :
                try:
                    if isRunningFunc() :
                        self.lineRead = (self.ser.readline()).decode() #precisa do decode se não ele retorna b'{valores}'
                        self.onSerialThreadUpdate(self.lineRead)
                        self.onThreadUpdateCheckFileWrite(self.lineRead)
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
                    self.ser.write(str.encode("2#"))

                    if self.saveFile == True :
                        self.saveFile = False
                    else :
                        self.saveFile = True    
            except : pass        

        def onSerialThreadUpdate( self, status ) : #onMyLongProcessUpdate( self, status ) :
            print( str(status) ) #print ("Process Update: %s" % (status))
            self.receviedInfo.set( str(status) ) #self.statusLabelVar.set( str.encode(status) )
            self.infoLabel.place( x = 20, y = 20)

        def onThreadUpdateCheckFileWrite( self, status ) :
            print( "Checking if write file" )
            if self.saveFile == True :
                self.logFile = open("logJanPy.txt","a+")
                self.logFile.write( str(status) + "\r" )
                self.logFile.close()

    root = Tk()    
    gui = UnitTestGUI( root )
    root.protocol( "WM_DELETE_WINDOW", gui.close )
    root.mainloop()

if __name__ == "__main__": 
    tkThreadingTest()