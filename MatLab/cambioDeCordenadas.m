function FP = cambioDeCordenadas(TH,R,I)
[X,Y] = pol2cart(TH,R);
FP = I(round(X),round(Y));