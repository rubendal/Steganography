# Steganography
C# Windows Forms application to apply steganography to image and audio files

```

Hide text or files inside pictures or wav files
Using LSB algorithm for wav files and a variation of LSB for images to increase file size limit
Can use two random algorithms with different performances and the linear algorithm which is the fastest
Text is encrypted with MD5, files are ciphered with xor cipher algorithm (https://github.com/rubendal/File-Cipher)
No external libraries used!
```




To Do:

* Improve file ciphering (https://github.com/rubendal/File-Cipher)
* Improve performance of random algorithm
