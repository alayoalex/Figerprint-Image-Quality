function Sth = espectro(I,R)

A = [];
[m,n] = size(I);
Dir = [pi/10 pi/9 pi/8 pi/7 pi/6 pi/5 pi/4 pi/3 pi/2 pi/2+pi/10 pi/2+pi/9 pi/2+pi/8 pi/2+pi/7 pi/2+pi/6 pi/2+pi/5 pi/2+pi/4 pi/2+pi/3 pi];
R = 1:256;

for i = fix(m/2):m
    for j = fix(n/2):n
        y = i - fix(m/2);
        x = j - fix(n/2);
        [TH1,r] = cart2pol(y,x);        
        
        if (abs(TH1 - TH) <= 0.005 && r <= min([m n]) && R == r)            
            A = [A I(i,j)];
        end
    end
end

Sth = A;
            
    