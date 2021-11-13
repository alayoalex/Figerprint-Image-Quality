Fs = 1000; % Frecuencia de muestreo: 1KHz
T = 1/Fs; % Período de muestreo
L = 1000; % Número de unidades
x = (0:L-1)*T; x=x'; % Vector de tiempo (x)
K=500; % Amplitud (unos)
a=ones(K,1); b=zeros(L-K,1); fx=[a; b];
plot(x,fx,'*')
