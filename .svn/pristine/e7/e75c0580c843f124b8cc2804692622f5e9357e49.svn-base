$(function () {
	//Checkbox
	$(".item-total-checkbox").each(function (index, item) {
		let parent = $(item);
		let table = parent.data("table");
		let child = ((table != undefined && table != null && table != "") ? table + " " : "") + parent.data("bind-to");
		$(parent).on("click", function () {
			let flag = parent.prop("checked");
			child = parent.data("bind-to");

			$(child).prop('checked', flag).change();
		});
		$(child).on("change", function () {
			let flag = true;
			for (let i = $(child).length - 1; i >= 0; i--) {
				if ($(child)[i].checked != flag) {
					flag = false;
					break;
				}
			}
			$(parent).prop('checked', flag);
		});
	});
})