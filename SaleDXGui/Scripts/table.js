function initTable(tableDom, isAnimate = true) {
    $(".item-total-checkbox").prop("checked", false);
	let table = $(tableDom);
	let theader = table.find("thead");
	var $th = $(theader).find('th');
	$th.css('transform', 'translateY(' + 0 + 'px)');
	let hiddenHead = $(theader).find(".hidden");
	$(hiddenHead).each(function (index, item) {
		$(item).children().each(function (i, c) {
			$(c).css("visibility", "hidden");
			$(c).attr("is-shown", false);
		});
		$($(item).attr("show-on-change")).on("change", function () {
			$(item).children().each(function (i, c) {
				if ($($(item).attr("show-on-change")).filter(':checked').length <= 0) {
					$(c).css("visibility", "hidden");
					$(c).attr("is-shown", false);
				} else {
					$(c).css("visibility", "visible");
					$(c).attr("is-shown", true);
				}
			});
		})
	});
    $(tableDom + " .hide").removeClass("hide");
    if (isAnimate) {
        sr.reveal(tableDom + " tr", {
            origin: 'bottom',
            scale: 1,
            distance: '50px',
            afterReveal: function (domEl) {
                $(domEl).attr("style", "")
            }
        }, 50);
    }
}

function sort(e, field) {
    var drt = $(e).attr('data-toggle-class');
    var direction = -1
    if (drt === "desc")
        direction = 0
    if (drt === "null")
        direction = 1
    var sort = listSorts.find(x => x.Field === field)
    if (sort) {
        var index = listSorts.indexOf(sort)
        sort.Direction = direction
        listSorts.splice(index, 1)
        listSorts = [sort].concat(listSorts)
    } else {
        listSorts = [{ Field: field, Direction: direction }].concat(listSorts)
    }
    listSorts = listSorts.filter(x => x.Direction != 0)
    search()
}
