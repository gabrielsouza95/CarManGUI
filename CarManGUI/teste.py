from Tkinter import *
import tkFont
import serial

win = Tk()

myFont = tkFont.Font(family = 'Microsoft Sans Serif', size = 8, weight = 'normal')
runComunication = False

class Teste:
  def rec_log(self):
    runComunication != runComunication

    if(runComunication == True):
      print("hello_world")
    

  def exitProgram():
    win.quit()  

win.title("CarManGUI")
win.geometry('420x320')

exitButton = Button(win, text = "Exit", font = myFont, comand = exitProgram, height = 2, width = 6)
exitButton.pack(side = BOTTOM)

recButton = Button(win, text = "Rec", font = myFont, comand = rec_log, height = 2, width = 6)
recButton.pack()