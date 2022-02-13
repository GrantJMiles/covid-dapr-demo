# Covid Testing Lab
Promise Pattern API making use of the State Store for Promises, then using the Rabbit MQ pub/sub for the eventual commands
[Extra] Send a CovidTestResult Event to the ESDB with NHS number and Date for further analysis
1. Recieves a patients name
1. Registers the patient by saving their details (nhs num -> CovidPatient{})
1. Gets the Current Advice Stats from the Government
1. Performs the calculation to get covid results
1. Publishes the CovidPositive and CovidNegative Events

Change Log
[13/02/2022]
AddDapr() does work but needed the other builder middlewares adding.  Added projections to ES for covid counts and percentages, could maybe link them rather than listening for the orig event. Added the publush event for a positive result. Nest steps are to write the consumer applciation for the test and trace postive contact. Or the worker to start posting lots of events through the stream
[02/02/22]
Looks like AddDapr() doesnt work for minimal API. need to make a new webapi and give it a go with the consumer seperated.
[23/01/22]
Added the FromBossy still not working
[22/01/22]
ES stream now being added, need to review the projection to get a "state" for counting positive/negative. Also, the NHS number is returning null all the time for some reason it isnt deserializing correctly.

[19/01/22]
All calculations complete. the consumer does all the work and now the result needs to be sent to EventStore and the projections for NhsNumber and Day/Week/Month

[18/01/22]
Started writing the consumer to subscribe to the event. Currently on the Covid processing and working out the logic to calulate the weight of getting a negative result. for example 14 day isolation period at lockdown level 5 ((14 * 5) / 100)
gives a 70% chance of negative. now need to factor in the vaccination factor

[16/01/22]
Added the Post/Put/Get endpoints for posting a covid patient to start to get their results.
Added a new component for State (REDIS) with a Prefix Strategy set to name so that when DAPR saves state it uses component name
Next Steps: Write a consumer for TestPatientCovidCommand in this API (for now) which registers the user and does the calculation to see if they have covid by calling the GovAdvice service then randomly calculating the result based on the advice. Then posts a Positive or Negative Command for another comsumer.