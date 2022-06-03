from email.mime import image
from multiprocessing.connection import wait
from tkinter import Image
from winreg import KEY_READ
import cv2 as cv
import cv2    
import numpy as np

def Click(event,x,y,flag, userdata) : 
    if(event == cv2.EVENT_LBUTTONDOWN):
        print('You have clicked')
        print(x,y)
        print(img[y,x])
    print('Done')

img = cv2.imread('image.png',1)
cv2.namedWindow('Window')
cv2.setMouseCallback('Window', Click)
cv2.imshow('Window',img)
cv2.waitKey(0);
