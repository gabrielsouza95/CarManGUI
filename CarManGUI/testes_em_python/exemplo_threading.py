import threading

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

def tkThreadingTest():

    from tkinter import Tk, Label, Button, StringVar
    from time import sleep

    class UnitTestGUI:

        def __init__( self, master ):
            self.master = master
            master.title( "Threading Test" )

            self.testButton = Button( 
                self.master, text="Blocking", command=self.myLongProcess )
            self.testButton.pack()

            self.threadedButton = Button( 
                self.master, text="Threaded", command=self.onThreadedClicked )
            self.threadedButton.pack()

            self.cancelButton = Button( 
                self.master, text="Stop", command=self.onStopClicked )
            self.cancelButton.pack()

            self.statusLabelVar = StringVar()
            self.statusLabel = Label( master, textvariable=self.statusLabelVar )
            self.statusLabel.pack()

            self.clickMeButton = Button( 
                self.master, text="Click Me", command=self.onClickMeClicked )
            self.clickMeButton.pack()

            self.clickCountLabelVar = StringVar()            
            self.clickCountLabel = Label( master,  textvariable=self.clickCountLabelVar )
            self.clickCountLabel.pack()

            self.threadedButton = Button( 
                self.master, text="Timer", command=self.onTimerClicked )
            self.threadedButton.pack()

            self.timerCountLabelVar = StringVar()            
            self.timerCountLabel = Label( master,  textvariable=self.timerCountLabelVar )
            self.timerCountLabel.pack()

            self.timerCounter_=0

            self.clickCounter_=0

            self.bgTask = BackgroundTask( self.myLongProcess )

            self.timer = TkRepeatingTask( self.master, self.onTimer, 1 )

        def close( self ) :
            print ("close")
            try: self.bgTask.stop()
            except: pass
            try: self.timer.stop()
            except: pass            
            self.master.quit()

        def onThreadedClicked( self ):
            print ("onThreadedClicked")
            try: self.bgTask.start()
            except: pass

        def onTimerClicked( self ) :
            print ("onTimerClicked")
            self.timer.start()

        def onStopClicked( self ) :
            print ("onStopClicked")
            try: self.bgTask.stop()
            except: pass
            try: self.timer.stop()
            except: pass                        

        def onClickMeClicked( self ):
            print ("onClickMeClicked")
            self.clickCounter_+=1
            self.clickCountLabelVar.set( str(self.clickCounter_) )

        def onTimer( self ) :
            print ("onTimer")
            self.timerCounter_+=1
            self.timerCountLabelVar.set( str(self.timerCounter_) )

        def myLongProcess( self, isRunningFunc=None ) :
            print ("starting myLongProcess")
            for i in range( 1, 10 ):
                try:
                    if not isRunningFunc() :
                        self.onMyLongProcessUpdate( "Stopped!" )
                        return
                except : pass   
                self.onMyLongProcessUpdate( i )
                sleep( 1.5 ) # simulate doing work
            self.onMyLongProcessUpdate( "Done!" )                

        def onMyLongProcessUpdate( self, status ) :
            print ("Process Update: %s" % (status))
            self.statusLabelVar.set( str(status) )

    root = Tk()    
    gui = UnitTestGUI( root )
    root.protocol( "WM_DELETE_WINDOW", gui.close )
    root.mainloop()

if __name__ == "__main__": 
    tkThreadingTest()