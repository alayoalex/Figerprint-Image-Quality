function v = conversion()

B = ones(20, 20, 2);
[m,n,o] = size(B);

for i = fix(m/2):m
    for j = fix(n/2):n
        y = i - fix(m/2);
        x = j - fix(n/2);
        
        %r = sqrt(i*i + j*j);
        %th = atan(j/i);
        
        [th,r] = cart2pol(y,x);       
        
        B(i,j,1) = fix(r);
        B(i,j,2) = th*180/pi;
    end
end

v = B;