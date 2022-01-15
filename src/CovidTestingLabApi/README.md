# Covid Testing Lab
Promise Pattern API making use of the State Store for Promises, then using the Rabbit MQ pub/sub for the eventual commands
[Extra] Send a CovidTestResult Event to the ESDB with NHS number and Date for further analysis
1. Recieves a patients name
1. Registers the patient by saving their details (nhs num -> CovidPatient{})
1. Gets the Current Advice Stats from the Government
1. Performs the calculation to get covid results
1. Publishes the CovidPositive and CovidNegative Events