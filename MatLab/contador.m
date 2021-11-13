function [x, y, cont] = contador(I)

cont = 0;
x = 0; y = 0;
for i = 1:364
    for j = 1:256
        if (I(i,j) > 13)
            cont = cont + 1;
            x=i;
            y=j;
        end
    end
end

