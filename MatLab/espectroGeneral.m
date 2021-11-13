function S = espectroGeneral(I)

B = size(A,2);
X = [];

for i = 1:B    
    X = [X; espectro(I,A(i))];    
end

S = X;