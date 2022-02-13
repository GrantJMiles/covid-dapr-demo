options({
    resultStreamName: "covid-result-counts",
    $includeLinks: false,
    reorderEvents: false,
    processingLag: 0
})

fromStream("covid-result-v1")
.when({
    $init: function() {
        return {
            postive: 0,
            negative: 0
        }
    },
    $any: function(s,e) {
        if (e.body.HasCovid){
            s.positive += 1;
        } else {
            s.negative += 1;
        }
    }
}).outputState()