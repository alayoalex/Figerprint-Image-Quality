A=fix(255*rand(20));
[f,c]=size(A);

for u=1:f
    for v=1:c
        FA(u,v)=0;
        for x=1:f
            for y=1:c
                FA(u,v)=FA(u,v) + A(x,y)*exp(-1i*2*pi*(u*x/f + v*y/c));
            end
        end
    end
end

FA=fftshift(FA);

figure, mesh(abs(log(FA+1)))
title('Transformada Discreta de Fourier (algoritmo propio)')