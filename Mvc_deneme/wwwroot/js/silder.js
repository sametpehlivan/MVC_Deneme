var models = [{ image: '1.jpg' },
              { image: '2.jpg' },
              { image: '3.jpg' },
              { image: '4.jpg' },
];

var index = 0;
var settings = {
    duration: '2000',
    random: true
}
var interval;
init(settings);

document.querySelector('#left').addEventListener('click', function () {
    index--;

    if (index < 0) {
        index = index * (-1)
        index = index % models.length;
        index = models.length - index;

    }
    getPicture(index);
});
document.querySelector('#right').addEventListener('click', function () {
    index++;
    if (index >= models.length) {
        index = index % models.length;
    }
    getPicture(index);
});
document.querySelectorAll('.arrow').forEach(function (item) {
    item.addEventListener('mouseenter', function () {
        clearInterval(interval);
    })

});

document.querySelectorAll('.arrow').forEach(function (item) {
    item.addEventListener('mouseleave', function () {
        init(settings);
    })

});

function getPicture(index) {
    document.querySelector('#_sliderImage').setAttribute('src',"~/lib/img/product/"+ models[index].image);
   
}
function init(settings) {
    var prev;
    interval = setInterval(function () {
        if (settings.random) {
            do {
                index = Math.floor(Math.random() * models.length);
            } while (index == prev);

            prev = index;
        }
        getPicture(index);
    }, settings.duration);

}


