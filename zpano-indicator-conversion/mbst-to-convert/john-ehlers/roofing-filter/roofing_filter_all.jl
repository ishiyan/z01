##############################
# TO DO
# 5-2. Dominant Cycle Measured by Zero Crossings of the Band-Pass Filter - Validated against TS up to DC portion
# 8-3. Autocorrelation Periodogram - Validated against TS up to Normalization
# Outstanding
# 9-1 onwards
##############################

using Statistics

@doc """
    HPLPRoofingFilter(x::Array{Float64}; HPPeriod::Int64=48, LPPeriod::Int64=10)::Array{Float64}

HP LP Roofing Filter - Equation 7-1
"""
function HPLPRoofingFilter(x::Array{Float64}; HPPeriod::Int64=48, LPPeriod::Int64=10)::Array{Float64}
    @assert HPPeriod<size(x,1) && HPPeriod>0 "Argument HPPeriod out of bounds."
    # Highpass filter cyclic components whose periods are shorter than 48 bars
    alpha1 = (cosd(360 / HPPeriod) + sind(360 / HPPeriod) - 1) / cosd(360 / HPPeriod)
    HP = zeros(size(x,1))
    @inbounds for i = 2:size(x,1)
    HP[i] = (1 - alpha1 / 2)*(x[i] - x[i-1]) + (1 - alpha1)*HP[i-1]
    end
    # Smooth with a Super Smoother Filter from equation 3-3
    a1 = exp(-1.414*3.14159 / LPPeriod)  # may wish to make this an argument in function
    b1 = 2*a1*cosd(1.414*180 / LPPeriod) # may wish to make this an argument in function
    c2 = b1
    c3 = -a1*a1
    c1 = 1 - c2 - c3
    LP_HP_Filt = zeros(size(x,1))
    @inbounds for i = 3:size(x,1)
        LP_HP_Filt[i] = c1*(HP[i] + HP[i-1]) / 2 + c2*LP_HP_Filt[i-1] + c3*LP_HP_Filt[i-2]
    end
    return LP_HP_Filt
end

@doc """
    ZeroMeanRoofingFilterK0(x::Array{Float64}; HPPeriod::Int64=48, Smooth::Int64=10)::Array{Float64}

Zero Mean Roofing Filter - Lag 0 - Equation 7-2
K0 = Lag 0
# Lag 0 Is Most Responsive
# Ehlers describes using Lag 0 and Lag 1 cross overs/unders as a signal trigger for buying / selling
"""
function ZeroMeanRoofingFilterK0(x::Array{Float64}; HPPeriod::Int64=48, Smooth::Int64=10)::Array{Float64}
    @assert HPPeriod<size(x,1) && HPPeriod>0 "Argument HPPeriod out of bounds."
    # Highpass filter cyclic components whose periods are shorter than 48 bars
    alpha1 = (cosd(360 / HPPeriod) + sind(360 / HPPeriod) - 1) /cosd(360 / HPPeriod)
    HP = zeros(size(x,1))
    @inbounds for i = 2:size(x,1)
        HP[i] = (1 - alpha1 / 2)*(x[i] - x[i-1]) +(1 - alpha1)*HP[i-1]
    end
    #Smooth with a Super Smoother Filter from equation 3-3
    a1 = exp(-1.414*3.14159 / Smooth)
    b1 = 2*a1*cosd(1.414*180 / Smooth)
    c2 = b1
    c3 = -a1*a1
    c1 = 1 - c2 - c3
    Zero_Mean_Filt = zeros(size(x,1))
    Zero_Mean_Filt2 = zeros(size(x,1))
    @inbounds for i = 3:size(x,1)
        Zero_Mean_Filt[i] = c1*(HP[i] + HP[i-1]) / 2 + c2*Zero_Mean_Filt[i-1] + c3*Zero_Mean_Filt[i-2]
        Zero_Mean_Filt2[i] = (1 - alpha1 / 2)*(Zero_Mean_Filt[i] - Zero_Mean_Filt[i-1]) + (1 - alpha1)*Zero_Mean_Filt2[i-1]
    end
    return Zero_Mean_Filt
end

@doc """
    ZeroMeanRoofingFilterK1(x::Array{Float64}; HPPeriod::Int64=48, Smooth::Int64=10)::Array{Float64}

Zero Mean Roofing Filter - Lag 1 - Equation 7-2
K1 = Lag 1
"""
function ZeroMeanRoofingFilterK1(x::Array{Float64}; HPPeriod::Int64=48, Smooth::Int64=10)::Array{Float64}
    @assert HPPeriod<size(x,1) && HPPeriod>0 "Argument HPPeriod out of bounds."
    # Highpass filter cyclic components whose periods are shorter than 48 bars
    alpha1 = (cosd(360 / HPPeriod) + sind(360 / HPPeriod) - 1) /cosd(360 / HPPeriod)
    HP = zeros(size(x,1))
    @inbounds for i = 2:size(x,1)
        HP[i] = (1 - alpha1 / 2)*(x[i] - x[i-1]) +(1 - alpha1)*HP[i-1]
    end
    #Smooth with a Super Smoother Filter from equation 3-3
    a1 = exp(-1.414*3.14159 / Smooth)
    b1 = 2*a1*cosd(1.414*180 / Smooth)
    c2 = b1
    c3 = -a1*a1
    c1 = 1 - c2 - c3
    Zero_Mean_Filt = zeros(size(x,1))
    Zero_Mean_Filt2 = zeros(size(x,1))
    @inbounds for i = 3:size(x,1)
        Zero_Mean_Filt[i] = c1*(HP[i] + HP[i-1]) / 2 + c2*Zero_Mean_Filt[i-1] + c3*Zero_Mean_Filt[i-2]
        Zero_Mean_Filt2[i] = (1 - alpha1 / 2)*(Zero_Mean_Filt[i] - Zero_Mean_Filt[i-1]) + (1 - alpha1)*Zero_Mean_Filt2[i-1]
    end
    return Zero_Mean_Filt2
end

@doc """
    RoofingFilterIndicator(x::Array{Float64}; LPPeriod::Int64=40,HPPeriod::Int64=80)::Array{Float64}

Roofing Filter As Indicator - Equation 7-3
"""
function RoofingFilterIndicator(x::Array{Float64}; LPPeriod::Int64=40,HPPeriod::Int64=80)::Array{Float64}
    @assert HPPeriod<size(x,1) && HPPeriod>0 "Argument HPPeriod out of bounds."
    # Highpass filter cyclic components whose periods are shorter than 48 (n) bars
    alpha1 = (cosd(.707*360 / HPPeriod) + sind(.707*360 /HPPeriod) - 1) / cosd(.707*360 / HPPeriod)
    HP = zeros(size(x,1))
    @inbounds for i = 3:length(x)
    HP[i] = (1 - alpha1 / 2)*(1 - alpha1 / 2)*(x[i] - 2*x[i-1] + x[i-2]) + 2*(1 - alpha1)*HP[i-1] - (1 - alpha1)*(1 - alpha1)*HP[i-2]
    end
    #Smooth with a Super Smoother Filter from equation 3-3
    a1 = exp(-1.414*3.14159 / LPPeriod)
    b1 = 2*a1*cosd(1.414*180 / LPPeriod)
    c2 = b1
    c3 = -a1*a1
    c1 = 1 - c2 - c3
    Roof_filt_Indicator = zeros(size(x,1))
    @inbounds for i = 3:length(x)
        Roof_filt_Indicator[i] = c1*(HP[i] + HP[i-1]) / 2 + c2*Roof_filt_Indicator[i-1] + c3*Roof_filt_Indicator[i-2]
    end
    return Roof_filt_Indicator
end
