var chart_options = {
    chart: {
        renderTo: 'hc_chart',
        marginTop: 40,
        type: 'spline',
        zoomType: 'x',
        x: -20
    },
    credits: {
        enabled: false
    },
    lang: {
        clearseries: "Clear all chart series",
        newwindow: "Open chart in new window"
    },
    rangeSelector: {
        buttons: [
                            { type: 'day', count: 3, text: '3d' },
                            { type: 'week', count: 1, text: '1w' },
                            { type: 'month', count: 1, text: '1m' },
                            { type: 'month', count: 6, text: '6m' },
                            { type: 'year', count: 1, text: '1y' },
                            { type: 'all', text: 'All' }
                        ]
    },
    title: {
        text: '',
        style: {
            display: 'none'
        }
    },
    subtitle: {
        text: '',
        style: {
            display: 'none'
        }
    },
    xAxis: {
        type: 'datetime',
        dateTimeLabelFormats: {
            month: '%e. %b',
            year: '%b'
        }
    },
    plotOptions: {
        area: {
            marker: {
                enabled: false
            }
        },
        areaspline: {
            marker: {
                enabled: false
            }
        },
        column: {
            marker: {
                enabled: false
            }
        },
        line: {
            marker: {
                enabled: false
            }
        },
        spline: {
            marker: {
                enabled: false
            }
        }
    },
    exporting: {
        buttons: {
            'clearseries': {
                _id: 'clearseries',
                _titleKey: "clearseries",
                symbol: 'circle',
                x: -30,
                symbolFill: '#FC911A',
                hoverSymbolFill: '#FC911A',
                onclick: function () {
                    hc_chart.clearseries();
                }
            },
            'newwindow': {
                _id: 'newwindow',
                _titleKey: "newwindow",
                symbol: 'square',
                x: -60,
                symbolFill: '#FC911A',
                hoverSymbolFill: '#FC911A',
                onclick: function () {

                    window.open("/pages/pop-up.aspx", "chart-pop-up", "width=820,height=620,scrollbars=0,menubar=0,location=0");
                }
            }
        }
    }
};