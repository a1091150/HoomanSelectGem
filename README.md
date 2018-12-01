# HoomanSelectGem
Generate data from the Screenshot.

This project is to get the small pictures from the source images, which is the screenshot from Tower of Savior(ToS).
There is an example in this project, a screenshot from Iphone 8+ with 9:16 screen.

The GUI is created by GTK# and C# on mac with Visual Studio Community. It is a cross-platform app.
It will create a (.csv) file which contains gem type of picture, gem image, and board image.

Create those files to train tensorflow model.

## 繁體中文：
輸入來自神魔之塔的截圖，輸出 珠子、版面、以及你自己辨識的珠子屬性。初步目標是訓練tensorflow model來辨識珠子。
## How to Use

First, Click the bottom-right button(Select Source Image) to select the origional image. 
The image should be 9:16 aspect ratio and more higher resolution than (180, 320).

![](https://github.com/a1091150/HoomanSelectGem/blob/master/project_screenshot/first.png)

After clicked, the program will create the folder with random guid name, bar**.png,and board.png files.

Second, click the second button (Select folder) and select the folder just created, it will show the picture.

![](https://github.com/a1091150/HoomanSelectGem/blob/master/project_screenshot/second.png)

Third, click the third button which enables the colorful button, and click the button with the same gem type match to the picture.

![](https://github.com/a1091150/HoomanSelectGem/blob/master/project_screenshot/third.png)

Finally, click Save button which creates (.csv) file.
