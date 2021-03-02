# RocketLandingAssistant

[LandingTrajectoryVerifier.cs](https://github.com/Smrecz/RocketLandingAssistant/blob/master/RocketLandingAssistant/LandingTrajectoryVerifier.cs) should be initialized with landing platform position and single instance should be used for all rocket position checks during single "landing session".

## Assumptions

1) Position check outside of landing area should result in exception (not covered by requirements - achieved with Value Object validation)
2) Landing platform size can vary but is constant during execution and can be configured only during initialization
3) Landing platform can only be rectangular in shape
