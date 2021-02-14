let chartColors = {
    blue: "rgb(54, 162, 235)",
    green: "rgb(75, 192, 192)",
    grey: "rgb(201, 203, 207)",
    orange: "rgb(255, 159, 64)",
    purple: "rgb(153, 102, 255)",
    red: "rgb(255, 99, 132)",
    yellow: "rgb(255, 205, 86)",
};

let color = Chart.helpers.color;

let radarChart = function (ctx, title, labelTitle, labelToData) {
    return new Chart(ctx, {
        type: 'radar',
        data: {
            labels: labelToData.map(l => l[0]),
            datasets: [{
                label: labelTitle,
                data: labelToData.map(l => parseInt(l[1])),
                backgroundColor: color(chartColors.red).alpha(0.2).rgbString(),
                borderColor: color(chartColors.red).alpha(0.7).rgbString()
            }]
        },
        options: {
            responsive: true,
            aspectRatio: 1,
            legend: {
                display: false
            },
            title: {
                display: true,
                text: title,
                fontSize: 15
            },
            scale: {
                ticks: {
                    stepSize: 1
                }
            },
            tooltips: {
                callbacks: {
                    title: (item, data) => item.map(i => data.labels[i.index]).join(',')
                }
            }
        }
    });
};

let lineChart = function (ctx, title, labelTitle, labelToData) {
    return new Chart(ctx, {
        type: 'line',
        data: {
            datasets: [{
                label: labelTitle,
                data: labelToData.map(l => {
                    return {
                        t: new Date(l[0]), y: parseInt(l[1])
                    };
                })
                // backgroundColor: color(chartColors.red).alpha(0.2).rgbString(),
                // borderColor: color(chartColors.red).alpha(0.7).rgbString()
            }]
        },
        options: {
            responsive: true,
            aspectRatio: 2,
            legend: {
                display: false
            },
            title: {
                display: true,
                text: title,
                fontSize: 15
            },
            scales: {
                xAxes: [{
                    display: true,
                    type: 'time',
                    time: {
                        unit: 'day',
                        displayFormats: {
                            'day': 'YYYY-MM-DD'
                        }
                    }
                }],
                yAxes: [{
                    ticks: {
                        stepSize: 1
                    }
                }]
            }
        }
    });
};


$(function () {
    let labelsProblemTypes = $('input[name="problem-types"]').val().split(',').map(s => s.split(':'));
    
    radarChart($('#radar-chart')[0], '题解——题目类型统计', "题目数量", labelsProblemTypes);
    
    let labelSolutionCount = $('input[name="problem-count"]').val().split(',').map(s => s.split('~'));
    
    lineChart($('#solution-count-chart')[0], '题解——数量统计', "题解总数", labelSolutionCount);
});