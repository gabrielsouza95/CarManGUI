from tkinter import *
import tkinter.font
import serial
import sys

##serial
ser = serial.Serial('COM4', 9600)#('/dev/ttyACM0', 9600)
##window
win = Tk()

win.title("CarManGUI")
win.geometry('420x320')#resizable(0,0) #win.
#win.wm_attributes("-topmost", 1)

myFont = tkinter.font.Font(family = 'Microsoft Sans Serif', size = 8, weight = 'normal')
#canvas = Canvas(win, width = 420, height = 320, bd = 0, lighlightthickness = 0)
global runComunication
runComunication = 0

global receviedInfo
receviedInfo = StringVar()
receviedInfo.set("wating connection...")

global infoLabel
infoLabel = Label(win, textvariable = receviedInfo, relief = RAISED) 
infoLabel.place( x = 20, y = 20)

def rec_log():
  global runComunication
  global receviedInfo
  
  if runComunication == 0 :
    runComunication = 1
    #receviedInfo.set("1")
    #infoLabel.place( x = 20, y = 20 )
  else:
    runComunication = 0
    #receviedInfo.set("0")
    #infoLabel.place( x = 20, y = 20 )
    
  if runComunication == 1 :
    print("hello_world")
    
def readser():
  global runComunication
  global receviedInfo
  
  if runComunication == 1 :
    receviedInfo.set(ser.readline())
    infoLabel.place( x = 20, y = 20 )
    after(1,readser)
  else :
    receviedInfo.set("Not Conected")
    infoLabel.place( x = 20, y = 20 )
    
def exitProgram():
    sys.exit()  

exitButton = Button(win, text = "Exit", font = myFont, command = exitProgram, height = 2, width = 6)
exitButton.place(x = 230, y = 226)

recButton = Button(win, text = "Rec", font = myFont, command = rec_log, height = 2, width = 6 )
recButton.place(x = 140, y = 226)

conButton = Button(win, text = "Conect", font = myFont, command = readser, height = 2, width = 6 )
conButton.place(x = 40, y = 226)

win.mainloop()