using MarketCycles
using CSV
using Statistics

@static if VERSION < v"0.7.0-DEV.2005"
    using Base.Test
else
    using Test
end

######################################################################
# Validate against John Ehlers Original TradeStation Easylanguage code
# Run against dummy data loaded in Tradestation
######################################################################

# Note - for some tests irratic results for the lead in period #

# 3-3 SuperSmoother Test
@testset "Ehlers" begin
    @testset "HP LP Roofing Filter - Equation 7-1" begin
        filename = joinpath(dirname(@__FILE__), "test_7-1_HP_LP_Roofing_Filter.csv")
        test = CSV.read( filename)
        dat = Float64.(test[:, :x])
        HP_LP_Roof_benchmark = Float64.(test[:,:HP_LP_Roofing_Filter])
        HP_LP_Roof = HPLPRoofingFilter(dat)
        HP_LP_Roof = round.(HP_LP_Roof;digits=2) # round same as tradestation output
        valid = ifelse.(HP_LP_Roof .== HP_LP_Roof_benchmark,1,0)
        @test sum(valid) == length(valid)-48 # 48 bar lead in period
    end

    @testset "Zero Mean Roofing Filter - Lag 0 - Equation 7-2" begin
        filename = joinpath(dirname(@__FILE__), "test_7-2_Zero_Mean_Roofing_Filter.csv")
        test = CSV.read( filename)
        dat = Float64.(test[:, :x])
        Zero_Mean_Roofing_Filter_lag_0_benchmark = Float64.(test[:,:Filt])
        Zero_Mean_Roofing_Filter_lag_0 = HPLPRoofingFilter(dat)
        Zero_Mean_Roofing_Filter_lag_0 = round.(Zero_Mean_Roofing_Filter_lag_0;digits=2) # round same as tradestation output
        valid = ifelse.(Zero_Mean_Roofing_Filter_lag_0 .== Zero_Mean_Roofing_Filter_lag_0_benchmark,1,0)
        @test sum(valid) == length(valid)-48 # 48 bar lead in period
    end

    @testset "Zero Mean Roofing Filter - Lag 1 - Equation 7-2" begin
        filename = joinpath(dirname(@__FILE__), "test_7-2_Zero_Mean_Roofing_Filter.csv")
        test = CSV.read( filename)
        dat = Float64.(test[:, :x])
        Zero_Mean_Roofing_Filter_lag_1_benchmark = Float64.(test[:,:Filt2])
        Zero_Mean_Roofing_Filter_lag_1 = ZeroMeanRoofingFilterK1(dat)
        Zero_Mean_Roofing_Filter_lag_1 = round.(Zero_Mean_Roofing_Filter_lag_1;digits=2) # round same as tradestation output
        valid = ifelse.(Zero_Mean_Roofing_Filter_lag_1 .== Zero_Mean_Roofing_Filter_lag_1_benchmark,1,0)
        @test sum(valid) == length(valid)-60 # 48 bar lead in period
    end

    @testset "Roofing Filter Indicator Equation 7-3" begin
        filename = joinpath(dirname(@__FILE__), "test_7-3_Roofing_Filter_Indicator.csv")
        test = CSV.read(filename)
        dat = Float64.(test[:, :x])
        Roofing_Filter_Indicator_benchmark = Float64.(test[:,:Filt])
        Roofing_Filter_Indicator = RoofingFilterIndicator(dat)
        Roofing_Filter_Indicator = round.(Roofing_Filter_Indicator;digits=2) # round same as tradestation output
        valid = ifelse.(Roofing_Filter_Indicator .== Roofing_Filter_Indicator_benchmark,1,0)
        @test sum(valid) == length(valid)-213 # 213 bar lead in period
    end
end
