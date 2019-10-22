from Tkinter import *
import tkFont
import serial

win = Tk()

myFont = tkFont.Font(family = 'Microsoft Sans Serif', size = 8, weight = 'normal')

class Teste:
  def hello_world(self):
    print "hello_world"

  def exitProgram()
  	win.quit()  

win.title("KarManGUI")
win.geometry('420x320')

exitButton = Button(win, text = "Exit", font = myFont, comand = exitProgram, height = 2, width = 6)
exitButton.pack(side = BOTTOM)

