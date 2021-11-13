% Crea figura
f = zeros(30,30);
f(5:24,13:17) = 1;
% Aplica la transformada de Fourier
F = fft2(f,256,256);
% Mueve el origen de la transformada al centro del gráfico de frecuencias
F1 = fftshift(F);
%Visualiza
imshow(log(abs(F1)),[-1 5]); colormap(jet); colorbar