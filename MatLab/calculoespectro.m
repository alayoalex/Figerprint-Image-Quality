function A = calculoespectro(I)

A = [];
[m,n] = size(I);

for r = 0:min(m/2, n/2)
    S = 0;
    cont = 0;
    %for th = 0:pi        
        [x,y] = pol2cart(2*pi/3,r);  
        j = fix(x + n/2);
        i = fix(y + m/2);
        %if (fix(x) > 1 && fix(y) > 1)
            S = S + I(i, j);
            cont = cont + 1;
        %end
    %end
    A = [A S];    
end
plot(A,'DisplayName','ans','YDataSource','ans');figure(gcf)
