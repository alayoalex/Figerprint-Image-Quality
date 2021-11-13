A=double(imread('C:\Users\Alexei\Documents\MATLAB\huellaprueba.tif','tif')); 
%imshow(uint8(A))
Fa=fftshift(fft2(A));
mesh(log(abs(Fa)))