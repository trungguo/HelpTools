(function ($) {
    "use strict";
    var sortState = ["null", "asc", "desc"]
    //Checkbox

    $(document).on("click", ".sortable", function (item) {
        let stateIdx = 0;
        let f = false;
        for (let i = 0; i < sortState.length; i++) {
            if ($(this).is("." + sortState[i])) {
                stateIdx = i + 1;
                f = true;
            }
        }
        if (f) {
            if (stateIdx > sortState.length - 1) {
                stateIdx = 0;
            }
        } else {
            stateIdx = 1;
        }


        if (stateIdx == 0) {
            $(this).removeClass("active");
            $(this).removeClass($(this).attr('data-class'));
        }
        if (stateIdx != 0) {
            $(this).addClass("active");
            $(this).removeClass($(this).attr('data-class'));
        }


        $(this).attr("data-class", sortState[stateIdx]);
        $(this).attr("data-toggle-class", sortState[stateIdx]);
    });

    $("[data-toggle='custom-grid-dropdown']").on("click", function () {
        let $self = $(this);
        let placeHolder = document.createElement("div");
        let dropdown = $self.parent().find(".dropdown-menu")[0];

        if ($self.attr("aria-expanded") == "true") {
            $(dropdown).html($("#custom-dropdown-placeholder").html());
            return;
        }

        $(placeHolder).attr("id", "custom-dropdown-placeholder");
        $(placeHolder).html("<div class='" + $(dropdown).attr("class") + "'>" + $(dropdown).html() + "</div>");
        let rect = this.getBoundingClientRect();
        $(placeHolder).css({ "display": "block", "position": "absolute", "top": rect.bottom + "px", "left": rect.right + "px", "z-index": "999" });
        document.body.appendChild(placeHolder);
        $($(placeHolder).children()[0]).addClass("d-block");
        $(dropdown).html("");
        $(document).on("mouseup", function (e) {
            var container = $(placeHolder);

            // if the target of the click isn't the container nor a descendant of the container
            if ((!$self.is(e.target) && !container.is(e.target)) && container.has(e.target).length === 0) {
                $(dropdown).html($(placeHolder).children()[0].innerHTML);
                $(placeHolder).remove();
                return;
            }
            return;
        });
        $(window).resize(function () {
            $self.parent().append($(placeHolder));
            return;
        });

        $($($self).closest(".scroll-y")).scroll(function () {
            $(dropdown).html($(placeHolder).children()[0].innerHTML);
            $(placeHolder).remove();
            return;
        });
        return;
    })

})(jQuery);

function initGrid(listContainer, isAnimated = true) {
    let listDom = $(listContainer);
    let headerDom = listDom.attr("data-header");
    let header = $($(headerDom).find(".list-header")[0]).children("div");

    $(headerDom + " .list-header").removeClass("hide");
    sr.reveal(headerDom + " .list-header", {
        origin: 'bottom',
        scale: 1,
        distance: '25px',
    }, 25);
    $(listContainer + " .list").removeClass("hide");
    if (isAnimated) {
        isAnimated = false;
        sr.reveal(listContainer + " .list-item", {
            origin: 'bottom',
            scale: 1,
            distance: '50px',
        }, 50);
    }

    initGridHeaderSwapper(headerDom);
    resizeColumnToFit(listDom, header);
}

function initGridInTab(listContainer) {
    let listDom = $(listContainer);
    let headerDom = listDom.attr("data-header");
    let header = $($(headerDom).find(".list-header")[0]).children("div");
    $(headerDom + " .list-header").removeClass("hide");
    $(listContainer + " .list").removeClass("hide");
    initGridHeaderSwapper(listContainer);
    setTimeout(function () {
        resizeColumnToFit(listDom, header);
    }, 160);
}

function resizeColumnToFit(listContainer, header) {
    resize(listContainer, header);
    $(window).resize(function () {
        resize(listContainer, header);
    });

    function resize(listContainer, header) {
        let listWidth = [];
        for (let i = 0; i < header.length; i++) {
            if ($(header[i]).is(".auto-fit")) {
                listWidth[i] = 0;
                continue;
            }
            if ($(header[i]).is(".sortable")) {
                listWidth[i] = $(header[i]).width() + 6;
            } else {
                listWidth[i] = $(header[i]).width();
            }
        }
        let bodyRow = $($(listContainer).find(".list-body")[0]).children("div");

        for (let i = 0; i < bodyRow.length; i++) {
            let cols = $(bodyRow[i]).children("div");

            for (let j = 0; j < cols.length; j++) {
                if (listWidth[j] < $(cols[j]).width() && listWidth[j] != 0) {
                    listWidth[j] = $(cols[j]).width();
                }
            }
        }

        for (let i = 0; i < header.length; i++) {
            if (listWidth[i] <= 0) continue;
            if ($(header[i]).is(".sortable")) {
                $(header[i]).width(listWidth[i] - 6);
            } else {
                $(header[i]).width(listWidth[i]);
            }

        }

        for (let i = 0; i < bodyRow.length; i++) {
            let cols = $(bodyRow[i]).children("div");
            for (let j = 0; j < cols.length; j++) {
                if (listWidth[j] <= 0) continue;
                $(cols[j]).width(listWidth[j]);
            }
        }

        if ($(listContainer).hasScrollBar()) {
            $($(header)[0]).parent().addClass("mr-5px");
        } else {
            $($(header)[0]).parent().removeClass("mr-5px");
        }
    }
}

function initGridHeaderSwapper(headerDom) {
    $(headerDom).attr("is-toolbar-shown", false);
    $(headerDom).find(".list-header-swapper").each(function (index, item) {
        let bind = $($(item).attr("data-bind-to"));
        let item_to_fire = $($(item).attr("data-item-to-fire"));
        item_to_fire.on("change", function () {
            if (item_to_fire.filter(':checked').length > 0) {
                if ($(headerDom).attr("is-toolbar-shown") == "false") {
                    $(headerDom).attr("is-toolbar-shown", true)
                    Swap2HtmlListHeader($(item), $(bind));
                }
            } else {
                $(headerDom).attr("is-toolbar-shown", false);
                Swap2HtmlListHeader($(item), $(bind));
            }
        });


    });

}
function Swap2HtmlListHeader(swapFrom, swapto) {
    let From = $(swapFrom).children('div:not(div[no-swap="true"])');
    let To = $(swapto).children('div:not(div[no-swap="true"])');
    for (let i = From.length - 1; i >= 0; i--) {
        $(swapto).append($(From[i]));
        $(swapFrom).splice(i);
    }
    for (let i = To.length - 1; i >= 0; i--) {
        $(swapFrom).append($(To[i]));
        $(swapto).splice(i);
    }
}
function SwapGridHeader(listContainer) {
    let listDom = $(listContainer);
    let headerDom = listDom.attr("data-header");
    let chkBox = $($(headerDom).find(".item-total-checkbox"));
    chkBox.prop("checked", false)
    chkBox.trigger("change");
    $(headerDom).find(".list-header-swapper").each(function (index, item) {
        let bind = $($(item).attr("data-bind-to"));
        if ($(headerDom).attr("is-toolbar-shown") == "false") {
            $(headerDom).attr("is-toolbar-shown", true)
            Swap2HtmlListHeader($(item), $(bind));
        } else {
            $(headerDom).attr("is-toolbar-shown", false);
            Swap2HtmlListHeader($(item), $(bind));
        }
    });
}

function initPager(listContainer, currentPage, pageSize, total, onclickFn) {
    let pageDisplayIfGreaterThan10 = 3;
    let curPage = parseInt(currentPage) ?? 1;
    let html = ''
    let pagerContainer = $(listContainer).attr("data-pager");
    let pageSz = parseInt(pageSize) ?? 20;
    let pageTotal = Math.ceil(total / pageSz);
    pageTotal = (pageTotal > 0) ? pageTotal : 1;
    curPage = (curPage >= pageTotal) ? pageTotal : curPage;
    curPage = (curPage <= 0) ? 1 : curPage;
    if (pageTotal < 11) {
        html += '<button class="btn btn-pager btn-sm no-bg no-shadow px-0 pager-prev" ' + ((curPage == 1) ? 'disabled' : '') + ' data-page-index="' + (curPage - 1) + '"> <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-chevron-left"> <polyline points="15 18 9 12 15 6"></polyline> </svg> </button>';
        html += '<div class="pagination pagination-sm mx-1">';
        for (let i = 1; i < pageTotal + 1; i++) {
            html += '<li class="' + ((i == curPage) ? "active" : "") + '"><a class="page" data-page-index="' + i + '">' + i + '</a></li>'
        }
        html += '</div>';
        html += '<button class="btn btn-pager btn-sm no-bg no-shadow px-0 pager-next" ' + ((curPage == pageTotal) ? 'disabled' : '') + ' data-page-index="' + (curPage + 1) + '"> <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-chevron-right"> <polyline points="9 18 15 12 9 6"></polyline> </svg> </button>';
    } else {
        html += '<button class="btn btn-pager btn-sm no-bg no-shadow px-0 pager-prev" ' + ((curPage == 1) ? 'disabled' : '') + ' data-page-index="' + (curPage - 1) + '"> <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-chevron-left"> <polyline points="15 18 9 12 15 6"></polyline> </svg> </button>';
        html += '<div class="pagination pagination-sm mx-1">';
        if (curPage > pageDisplayIfGreaterThan10) {
            if (curPage - (pageDisplayIfGreaterThan10 + 1) > 0) {
                html += '<li class=""><a class="" >...</a></li>';
            }

            for (let i = curPage - pageDisplayIfGreaterThan10; i < curPage; i++) {
                html += '<li class=""><a class="page" data-page-index="' + i + '">' + i + '</a></li>'
            }
        } else {
            for (let i = 1; i < curPage; i++) {
                html += '<li class=""><a class="page" data-page-index="' + i + '">' + i + '</a></li>'
            }
        }
        if (pageTotal - curPage >= pageDisplayIfGreaterThan10) {
            for (let i = curPage; i < curPage + (pageDisplayIfGreaterThan10 + 1); i++) {
                html += '<li class="' + ((i == curPage) ? "active" : "") + '"><a class="page" data-page-index="' + i + '">' + i + '</a></li>'
            }
            if (pageTotal - curPage - pageDisplayIfGreaterThan10 > 0) {
                html += '<li class=""><a class="">...</a></li>';
            }
        } else {
            for (let i = curPage; i <= curPage; i++) {
                html += '<li class="' + ((i == curPage) ? "active" : "") + '"><a class="page" data-page-index="' + i + '">' + i + '</a></li>'
            }
        }
        html += '</div>';
        html += '<button class="btn btn-pager btn-sm no-bg no-shadow px-0 pager-next" ' + ((curPage == pageTotal) ? 'disabled' : '') + ' data-page-index="' + (curPage + 1) + '"> <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-chevron-right"> <polyline points="9 18 15 12 9 6"></polyline> </svg> </button>';
    }

    if (pageTotal >= 11) {
        html = '<button class="btn btn-pager btn-sm no-bg no-shadow px-0 pager-first" ' + ((curPage == 1) ? 'disabled' : '') + ' data-page-index="' + 1 + '"> <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-chevrons-left mx-2"><polyline points="11 17 6 12 11 7"></polyline><polyline points="18 17 13 12 18 7"></polyline></svg> </button>' + html;
        html += '<button class="btn btn-pager btn-sm no-bg no-shadow px-0 pager-last" ' + ((curPage == pageTotal) ? 'disabled' : '') + ' data-page-index="' + pageTotal + '"> <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-chevrons-right mx-2"><polyline points="13 17 18 12 13 7"></polyline><polyline points="6 17 11 12 6 7"></polyline></svg> </button>';
    }

    $(pagerContainer).html(html);

    let button = $(pagerContainer).find(".btn-pager");
    let page = $(pagerContainer).find(".page");
    $($(pagerContainer).parent().find(".table-amount-count")).html(total.toString());

    $(button).on("click", function (ev, e) {
        ev.preventDetault;
        let pageIndex = $(this).attr("data-page-index");
        if (pageIndex != null && pageIndex != undefined && pageIndex != "") {
            onclickFn(parseInt($(this).attr("data-page-index")));
        }

    })
    $(page).on("click", function (ev, e) {
        ev.preventDetault;
        onclickFn(parseInt($(this).attr("data-page-index")));
    })
}

(function ($) {
    $.fn.hasScrollBar = function () {
        if (!this.get(0)) {
            return false;
        }
        return this.get(0).scrollHeight > this.height();
    }
})(jQuery);