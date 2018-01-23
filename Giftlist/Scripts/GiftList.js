$(function () {

	$('#CurrentUserName').click(function () {
		//stop event from bubbling up to parent, else it fades out:
		event.stopPropagation();
		$('#AccountOptions').toggle(200);
	});

	$('body').click(function () {
		$('#AccountOptions').hide(200);
	});

});



function ShowList(userId) {
	$.ajax({
		url: 'List/GetListByUserId/',
		method: 'POST',
		data: { userId: userId },
		success: function (data) {
			RenderList(data);
		},
		error: function (data) {
			console.log(document.write(data.responseText));
		}
	});
}

function RenderList(data) {
	//console.log(data);
	var liststring = "";
	liststring += '<div class="List">';
	if (data.IsUser) {
		liststring += '<a class="AddNewItem" href="List/New">Add New Item</a>';
	}
	liststring += '<table><tr class="header"><td class="td1">Item</td><td class="td2">Description</td><td class="td3">Priority</td><td class="td4">Status</td></tr>';
	if (data.Items) {
		for (var i = 0; i < data.Items.length; i++) {
			var isFullClaim = false;
			liststring += '<tr><td class="td1">';
			if (data.Items[i].Url != null) {
				liststring += '<a href="' + data.Items[i].Url + '" target="_blank">' + data.Items[i].Title + '</a>';
			} else {
				liststring += data.Items[i].Title;
			}
			liststring += '</td><td class="td2">' + data.Items[i].Description +
				'</td><td class="td3">' + data.Items[i].Priority +
				'</td><td class="td4">';
			if (!data.IsUser) {
				// Show Claim stuff
				if (data.Items[i].ItemClaimCount > 0) {
					for (var j = 0; j < data.Items[i].ItemClaims.length; j++) {
						liststring += '<span class="ItemClaimed" data-id=' + data.Items[i].ItemId + '>';
						liststring += data.Items[i].ItemClaims[j].ClaimType + ' claim';
						if (data.Items[i].ItemClaims[j].Comment.length > 0) {
							liststring += '<span class="ClaimComment" data-id=' + data.Items[i].ItemClaims[j].ClaimId + '>i</span>';
						}
						liststring += '<br/>(' + data.Items[i].ItemClaims[j].DateClaimed + ' by ' + data.Items[i].ItemClaims[j].ClaimedBy + ')</span><br/>';
						liststring += '<div class="comment" id="Comment' + data.Items[i].ItemClaims[j].ClaimId + '">' + data.Items[i].ItemClaims[j].Comment + '</div>';
						isFullClaim = (data.Items[i].ItemClaims[j].ClaimType == 'Full');
					}
				}
				if (!isFullClaim) {
					if (data.Items[i].ItemClaimCount > 0) {
						liststring += '<span class="ShowClaimForm" data-id=' + data.Items[i].ItemId + '>Add a claim</span>'
					} else {
						liststring += '<span class="ShowClaimForm" data-id=' + data.Items[i].ItemId + '>Claim this item</span>'
					}
				}
			}
			else {
				// User is looking at own list; just show edit/delete
				liststring += '<a href="List/Edit/' + data.Items[i].ItemId + '">Edit</a> | <a href="List/Delete/' + data.Items[i].ItemId + '">Delete</a>';
			}
			liststring += '</td></tr>';
		}
	}

	liststring += '</table></div>';

	$('.Content').html(liststring);
}