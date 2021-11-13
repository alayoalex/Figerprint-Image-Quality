B = double(imread('Fourier vs Huellas/0 copia.tif','tif'));
B1 = B(:,:,1);
I = log(1 + abs(fftshift(fft2(B1))));
%I = abs(fftshift(fft2(B1)));
%I = abs(imag(fftshift(fft2(B1))));
mesh(I); colormap(gray);