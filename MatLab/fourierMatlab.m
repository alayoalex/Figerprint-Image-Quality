FA_mat=fft2(A); 
FA_mat=fftshift(FA_mat); % Colocamos la frecuencia cero en el centro del
% espectro 
figure, mesh(abs(log(FA_mat+1))) 
title('Transformada Discreta de Fourier (funcion de Matlab "fft2")')