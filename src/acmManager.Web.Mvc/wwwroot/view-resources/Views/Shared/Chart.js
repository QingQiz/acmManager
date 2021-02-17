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

function random_rgba() {
    let o = Math.round, r = Math.random, s = 255;
    return 'rgba(' + o(r()*s) + ',' + o(r()*s) + ',' + o(r()*s) + ',' + r().toFixed(1) + ')';
}

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
    let getDate = function (s) {
        let date = s.split('/').map(n => parseInt(n));
        return new Date(date[0], date[1] - 1, date[2], date[3], date[4], date[5]);
    }
    return new Chart(ctx, {
        type: 'line',
        data: {
            datasets: [{
                label: labelTitle,
                data: labelToData.map(l => {
                    return {
                        t: getDate(l[0]), y: parseInt(l[1])
                    };
                }),
                backgroundColor: color(chartColors.blue).alpha(0.2).rgbString(),
                borderColor: color(chartColors.blue).alpha(0.7).rgbString()
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

let doughnutChart = function (ctx, title, labelTitle, labelToData) {
    let segment;
    let labelColor = labelToData.map(_ => color(random_rgba()));
    return new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: labelToData.map(l => l[0]),
            datasets: [{
                label: labelTitle,
                data: labelToData.map(l => parseInt(l[1])),
                backgroundColor: labelColor.map(c => c.alpha(0.8).rgbaString()),
                hoverBackgroundColor: labelColor.map(c => c.alpha(1).rgbaString())
            }],
            
            
        },
        options: {
            responsive: true,
            aspectRatio: 1,
            title: {
                display: true,
                text: title,
                fontSize: 15
            },
            // see: https://stackoverflow.com/a/59919300/13442887
            onHover: function (evt, elements) {
                if (elements && elements.length) {
                    segment = elements[0];
                    this.chart.update();
                    selectedIndex = segment["_index"];
                    segment._model.outerRadius += 10;
                } else {
                    if (segment) {
                        segment._model.outerRadius -= 10;
                    }
                    segment = null;
                }
            },
            layout: {
                padding: 30
            }
        }
    });
}

let selectValByName = function (name, separator) {
    return $(`input[name="${name}"]`).val().split(',').map(s => s.split(separator))
};

$(function () {
    let labelsProblemTypes = selectValByName("problem-types", ':');

    radarChart($('#radar-chart')[0], '题解——题目类型统计', "题目数量", labelsProblemTypes);

    let labelSolutionCount = selectValByName('problem-count', '~')

    lineChart($('#solution-count-chart')[0], '题解——数量统计', "题解总数", labelSolutionCount);
    
    let labelArticleCount = selectValByName('article-count', '~');
    
    lineChart($('#article-count-chart')[0], '文章——数量统计', "文章总数", labelArticleCount);
    
    let labelCertificate = selectValByName('certificate-count', '~')
    
    doughnutChart($('#certificate-count-chart')[0], '证书统计', '数量', labelCertificate);
});
