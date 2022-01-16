# Covid Testing Lab
Promise Pattern API making use of the State Store for Promises, then using the Rabbit MQ pub/sub for the eventual commands
[Extra] Send a CovidTestResult Event to the ESDB with NHS number and Date for further analysis
1. Recieves a patients name
1. Registers the patient by saving their details (nhs num -> CovidPatient{})
1. Gets the Current Advice Stats from the Government
1. Performs the calculation to get covid results
1. Publishes the CovidPositive and CovidNegative Events

Change Log

[16/01/22]
Added the Post/Put/Get endpoints for posting a covif patient to start to get their results.
Added a new component for State (REDIS) with a Prefix Strategy set to name so that when DAPR saves state it uses component name
Next Steps: Write a consumer for TestPatientCovidCommand in this API (for now) which registers the user and does the calculation to see if they have covid by calling the GovAdvice service then randomly calculating the result based on the advice. Then posts a Positive or Negative Command for another comsumer.