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
    @testset "Super Smoother - Equation 3-3" begin
        filename = joinpath(dirname(@__FILE__), "test_3-3_Supersmoother.csv")
        test = CSV.read( filename)
        dat = Float64.(test[:, :x])
        super_smoother_benchmark = Float64.(test[:,:Ten_Period_Supersmoother])
        Super_Smoother = SuperSmoother(dat,n=10)
        Super_Smoother = round.(Super_Smoother; digits=2) # round same as tradestation output
        valid = ifelse.(Super_Smoother .== super_smoother_benchmark,1,0)
        valid = valid[28:length(valid)] # remove indicator lead in period
        @test sum(valid) == length(valid)
        # Passed but takes 28 lead in bars to do so
    end
end
