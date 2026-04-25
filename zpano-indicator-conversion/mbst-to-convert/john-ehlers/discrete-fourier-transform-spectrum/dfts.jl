using Statistics

@doc """
DFTS(x::Array{Float64}; min_lag::Int64=1, max_lag::Int64=48, LPLength::Int64=10, HPLength::Int64=48)::Array{Float64}

Discrete Fourier Transform Sprectral Estimate - Equation 9-1
"""
function DFTS(x::Array{Float64}; min_lag::Int64=1, max_lag::Int64=48, LPLength::Int64=10, HPLength::Int64=48)::Array{Float64}
        @assert HPLength<size(x,1) && HPLength>0 "Argument n out of bounds."
# Highpass filter cyclic components whose periods are shorter than 48 bars
    alpha1 = (cosd(.707*360 / HPLength) + sind(.707*360 / HPLength) - 1) / cosd(.707*360 / HPLength)
    HP = zeros(size(x,1))
    @inbounds for i = 3:size(x,1)
        HP[i] = (1 - alpha1 / 2)*(1 - alpha1 / 2)*(x[i] - 2*x[i-1] +x[i-2]) + 2*(1 - alpha1)*HP[i-1] - (1 - alpha1)*(1 - alpha1)*HP[i-2]
    end
    # Smooth with a Super Smoother Filter from equation 3-3
    a1 = exp(-1.414*3.14159 / LPLength)
    b1 = 2*a1*cosd(1.414*180 / LPLength)
    c2 = b1
    c3 = -a1*a1
    c1 = 1 - c2 - c3
    Filt = zeros(size(x,1))
    @inbounds for i = 3:size(x,1)
        Filt[i] = c1*(HP[i] + HP[i-1]) / 2 + c2*Filt[i-1] + c3*Filt[i-2]
    end

    # Initialize matrix
    CosinePart = zeros(size(x,1), max_lag)
    SinePart = zeros(size(x,1), max_lag)
    Pwr = zeros(size(x,1), max_lag)
    # This is the DFT
    @inbounds for j = min_lag:max_lag
    @inbounds for k = 1:max_lag
        lagged_filt = [fill(0,k); Filt[1:length(Filt)-k]]
        CosinePart[:,j] .= CosinePart[:,j] .+ lagged_filt .* cosd(360 * k / j) / j
        SinePart[:,j] .= SinePart[:,j] .+ lagged_filt .* sind(360 * k / j) / j
        Pwr[:,j] .= CosinePart[:,j] .* CosinePart[:,j] .+ SinePart[:,j] .* SinePart[:,j]
        end
    end
    # Find Maximum Power Level for Normalization
    # Note difers from TS output
    MaxPwr = zeros(size(x,1), max_lag)
    @inbounds for j = min_lag:max_lag
    @inbounds for i = 2:size(x,1)
    MaxPwr[i,j]  = .995*MaxPwr[i-1,j]
        if Pwr[i,j]  > MaxPwr[i,j]
         MaxPwr[i,j] = Pwr[i,j]
        end
    end
    end

#+_+_+_+_+_+_+_+_+_+_+_+_ unable to validate the below against TS
    #Normalize Power Levels and Convert to Decibels
    @inbounds for j = min_lag:max_lag
        @inbounds for i = 1:size(x,1)
            if MaxPwr[i,j] != 0
                Pwr[i,j] = Pwr[i,j] / MaxPwr[i,j]
                end
            end
        end

    # Compute the dominant cycle using the CG of the spectrum
    Spx = zeros(size(x,1))
    Sp = zeros(size(x,1))
    for j = min_lag:max_lag
        Spx .= ifelse.(Pwr[:,j] .>= 0.5, Spx .+ j .* Pwr[:,j],Spx)
        Sp .= ifelse.(Pwr[:,j] .>= 0.5,Sp .+ Pwr[:,j],Sp)
    end

    DominantCycle = zeros(size(x,1))
    for i = 2:size(x,1)
        if Sp[i] != 0
            DominantCycle[i] = Spx[i] / Sp[i]
        else
            DominantCycle[i] = DominantCycle[i-1]  # if its zero carry forwrd previous value
            end
        end
        return DominantCycle
end
