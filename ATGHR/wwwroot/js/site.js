var divisions;
var roles;
var users;
var offices;
var userRelationshipTypes;
var permissions;
var performanceScores;
var performanceFields;
var performanceTextTypes;
var divisionPerformanceFieldDescriptions;
var modal;
var temp_id = -1;

$(document).ready(function () {
	$('.ui.sidebar').sidebar('attach events', '.toc.item');
	$('.menu .item').tab();
	$('.dropdown').dropdown();
	accordionCleaner();
	$('.ui.checkbox').checkbox();

	// Back-to-top-button on each tab body
	$('button.back-to-top').on('click', function () {
		$('html,body').animate({ scrollTop: 0 }, 'slow');
		document.body.scrollTop = 0;
		document.documentElement.scrollTop = 0;
	});

});

function loadLookups(params) {
	params.callback = typeof params.callback !== 'undefined' ? params.callback : function (){};
	$.ajax({
		type: 'GET',
		url: '/api/lookup/alllookups',
		success: function (data) {
			roles = data.roles;
			divisions = data.divisions;
			users = data.users;
			offices = data.atgOffices;
			userRelationshipTypes = data.userRelationshipTypes;
			permissions = data.permissions;
			performanceScores = data.performanceScores;
			performanceFields = data.performanceFields;
			performanceTextTypes = data.performanceTextTypes;
			divisionPerformanceFieldDescriptions = data.divisionPerformanceFieldDescriptions;
			performanceFieldExampleCategories = data.performanceFieldExampleCategories;
			setTimeout(params.callback, 0);
		}
	});
}

var pendingRequest = 0;
function addPendingRequest() {
	pendingRequest++;
}

function subtractPendingRequest() {
	pendingRequest--;
}

function allowRequest() {
	return pendingRequest <= 0;
}

var hashes = [];
var hashPossible = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';

function createHash(length) {
	var hash = '';
	while (hash === '' || hashes.includes(hash)) {
		for (var i = 0; i < (typeof length !== 'undefined' ? length : 5); i++) {
			hash += hashPossible.charAt(Math.floor(Math.random() * hashPossible.length));
		}
	}
	hashes.push(hash);
	return hash;
}

$.fn.dataParent = function () {
	return this.parents('[data-serialize]').first();
};

$.fn.firstDataChildren = function (parent) {
	return this.filter(function () { return $(this).dataParent().is(parent); });
};

$.fn.firstDataChildrenNonText = function (parent) {
	return this.filter(function () { return $(this).parents('.performance_text').length === 0 && $(this).dataParent().is(parent); });
};

$.fn.firstDataChildrenText = function (parent) {
	return this.filter(function () { return $(this).parents('.performance_text').length > 0 && $(this).dataParent().is(parent); });
};

$.fn.dataSerialized = function () {
	return this.dataParent().attr('data-serialize') === 'true';
};

$.fn.filterSerialized = function () {
	return this.filter(function () {
		return $(this).dataSerialized();
	});
};

$.fn.InputMaskValidator = function () {
	var valFields = $(this).find('input[data-inputmask],textarea[data-inputmask],input[data-inputmask-regex],textarea[data-inputmask-regex]');
	var failed = false;
	$(valFields).removeClass('error');
	$(valFields).each(function (index) {
		if (!$(this).inputmask('isComplete') && !$(this).val() === '' || $(this).attr('optional')) {
			$(this).addClass('error');
			failed = true;
		}
	});
};

// general post data function
function ajSubmit(params) {
	params.type = typeof params.type !== 'undefined' ? params.type : 'POST';
	params.dataType = typeof params.dataType !== 'undefined' ? params.dataType : 'json';
	params.contentType = typeof params.contentType !== 'undefined' ? params.contentType : 'application/json';
	params.data = typeof params.data !== 'undefined' ? params.data : null;
	params.url = typeof params.url !== 'undefined' ? params.url : '';
	params.timeout = typeof params.timeout !== 'undefined' ? params.timeout : 0;
	params.showLoading = typeof params.showLoading !== 'undefined' ? params.showLoading : true;
	params.workingMessage = typeof params.workingMessage !== 'undefined' ? params.workingMessage : 'Working';
	params.successMessage = typeof params.successMessage !== 'undefined' ? params.successMessage : 'Success';
	params.successClear = typeof params.successClear !== 'undefined' ? params.successClear : true;
	params.successClearWait = typeof params.successClearWait !== 'undefined' ? params.successClearWait : 2000;
	params.successFunction = typeof params.successFunction !== 'undefined' ? params.successFunction : function () { };
	params.successFunctionWait = typeof params.successFunctionWait !== 'undefined' ? params.successFunctionWait : 0;
	params.failedMessage = typeof params.failedMessage !== 'undefined' ? params.failedMessage : 'Failed';
	params.failedClear = typeof params.failedClear !== 'undefined' ? params.failedClear : true;
	params.failedClearWait = typeof params.failedClearWait !== 'undefined' ? params.failedClearWait : 3000;
	params.failedFunction = typeof params.failedFunction !== 'undefined' ? params.failedFunction : function () { };
	params.failedFunctionWait = typeof params.failedFunctionWait !== 'undefined' ? params.failedFunctionWait : 0;
	params.completeFunction = typeof params.completeFunction !== 'undefined' ? params.completeFunction : function () { };
	params.completeFunctionWait = typeof params.completeFunctionWait !== 'undefined' ? params.completeFunctionWait : 0;

	$.ajax({
		type: params.type,
		url: params.url,
		data: params.data,
		dataType: params.dataType,
		contentType: params.contentType,
		timeout: params.timeout,
		async: true,
		cache: false,
		success: function (data) {
			if (typeof data.success !== 'undefined' && data.success === false) {
				modal.addClass('modal_error');
				modal.text(params.failedMessage);
				setTimeout(function () { params.failedFunction(data); }, params.failedFunctionWait);
				if (params.failedClear) {
					setTimeout(function () { modal.fadeOut(500, 'swing', function () { modal.remove(); }); }, params.failedClearWait);
				}
			} else {
				modal.addClass('modal_success');
				modal.text(params.successMessage);
				setTimeout(function () { params.successFunction(data); }, params.successFunctionWait);
				if (params.successClear) {
					setTimeout(function () { modal.fadeOut(500, 'swing', function () { modal.remove(); }); }, params.successClearWait);
				}
			}
		},
		error: function (request, status, err) {
			modal.addClass('modal_error');
			modal.text(params.failedMessage);
			setTimeout(function () { params.failedFunction({ success: false, errorMessage: status + ' - ' + err }); }, params.failedFunctionWait);
			if (params.failedClear) {
				setTimeout(function () { modal.fadeOut(500, 'swing', function () { modal.remove(); }); }, params.failedClearWait);
			}
		},
		beforeSend: function () {
			if (typeof modal === 'object') {
				modal.remove();
			}
			modal = $('<div class="modal" style="display: none;"></div>').appendTo($('body')).text(params.workingMessage).fadeIn(100);
		},
		complete: function () {
			setTimeout(params.completeFunction, params.completeFunctionWait);
		}
	});
}

// Sets status bar styling classes
function setStatus(parent, employee, manager, lead, hr) {
	var overall = 1;
	//if (employee && manager && lead && hr) {
	//	// if everyone has seen and approved the sheet, the status bar is all blue
	//	overall = 2;
	//	parent.find('.status').html('<p>Status: Done');
	//} else if (overall === 1) {
	//	parent.find('.status').html(
	//		employee && manager && lead ? '<p>Status: Lead approved' :
	//			employee && manager ? '<p>Status: Manager approved' :
	//				employee  ? '<p>Status: Employee approved' : '' + '.</p>');
	//}

	parent.find('.sheet_progress_bar .column:nth-child(1) div').addClass(overall <= 1 ? employee ? 'ready' : 'attention' : overall === 2 ? 'done' : 'default').popup({
		hoverable: true,
		position: 'top center',
		variation: 'wide',
		html: '<p>Employee ' + (employee ? 'completed' : 'pending') + '.</p>'
	});

	parent.find('.sheet_progress_bar .column:nth-child(2) div').addClass(overall === 1 ? manager ? 'ready' : employee ? 'attention' : 'default' : overall === 2 ? 'done' : 'default').popup({
		hoverable: true,
		position: 'top center',
		variation: 'wide',
		html: '<p>Manager ' + (manager ? 'completed' : 'pending') + '.</p>'
	});

	parent.find('.sheet_progress_bar .column:nth-child(3) div').addClass(overall === 1 ? lead ? 'ready' : manager ? 'attention' : 'default' : overall === 2 ? 'done' : 'default').popup({
		hoverable: true,
		position: 'top center',
		variation: 'wide',
		html: '<p>Lead ' + (lead ? 'completed' : 'pending') + '.</p>'
	});

	parent.find('.sheet_progress_bar .column:nth-child(4) div').addClass(overall === 1 ? hr ? 'ready' : lead ? 'attention' : 'default' : overall === 2 ? 'done' : 'default').popup({
		hoverable: true,
		position: 'top center',
		variation: 'wide',
		html: '<p>HR ' + (hr ? 'completed' : 'pending') + '.</p>'
	});
}

function getLookup(value, valueName, lookupObject) {
	return lookupObject.find(function (item) {
		return item[valueName] === value;
	});
}

// Division Translator
function findDivision(id) {
	return divisions.find(function (division) {
		return division.divisionId === parseInt(id);
	});
}

function findDivisionByName(name) {
	return divisions.find(function (division) {
		return typeof division.parentDivisionId === 'undefined' && division.name === name.toString();
	});
}

function findPracticeByName(name) {
	return divisions.find(function (division) {
		return typeof division.parentDivisionId !== 'undefined' && division.name === name.toString();
	});
}

// Role Translator
function findRole(id) {
	return roles.find(function (role) {
		return role.roleId === parseInt(id);
	});
}

// User Translator
function findUser(id) {
	return users.find(function (user) {
		return user.userId === parseInt(id);
	});
}

// Sort all users by full name (first name first)
function sortUsers() {
	return users.sort(function (a, b) {
		var aName = a.name.toLowerCase();
		var bName = b.name.toLowerCase();
		return aName < bName ? -1 : aName > bName ? 1 : 0;
	});
}

// Performance Field Translator
function findField(id) {
	return performanceFields.find(function (performanceField) {
		return performanceField.performanceFieldId === parseInt(id);
	});
}

function findFieldByYearAndName(year, name) {
	return performanceFields.find(function (performanceField) {
		return performanceField.name === String(name) && performanceField.year === parseInt(year);
	});
}

// Performance Score Translator
function findScore(id) {
	return performanceScores.find(function (performanceScore) {
		return performanceScore.performanceScoreId === parseInt(id);
	});
}

function findTextType(id) {
	return performanceTextTypes.find(function (performanceTextType) {
		return performanceTextType.performanceTextTypeId === parseInt(id);
	});
}

function findTextTypeByName(name) {
	return performanceTextTypes.find(function (performanceTextType) {
		return performanceTextType.name.toUpperCase() === name.toUpperCase();
	});
}

function findDescription(divisionId, performanceFieldId) {
	return divisionPerformanceFieldDescriptions.find(function (description) {
		return description.divisionId === parseInt(divisionId)
			&& description.performanceFieldId === parseInt(performanceFieldId);
	});
}

// General function that prevents default page submits or bubbling.
function genPrevention(event) {
	event.stopPropagation();
	event.preventDefault();
}

function titleCase(string) {
	string = string.toLowerCase().split(' ');
	for (var i = 0; i < string.length; i++) {
		string[i] = string[i].charAt(0).toUpperCase() + string[i].slice(1);
	}
	return string.join(' ');
}

function formatInputDate(dateString) {
	var date = new Date(dateString);
	var day = (date.getDate() < 10 ? '0': '') + date.getDate();
	var month = (date.getMonth() < 9 ? '0': '') + (date.getMonth() + 1);
	var year = date.getFullYear();
	return year + '-' + month + '-' + day;
}

function formatDateFull(dateString) {
	var date = new Date(dateString);
	var formattedDate = Intl.DateTimeFormat('en-GB', {
		year: 'numeric',
		month: 'long',
		day: 'numeric',
		hour: 'numeric',
		minute: 'numeric',
		hour12: true
	}).format(date);

	return formattedDate;
}

function createPropertyName(heading) {
	var property = heading.replace(/[^A-z0-9]/ig, "");
	return property;
}

// creates supervisor cards for review sheet statuses and goal sheet statuses.
function createUserCard(params) {
	params.name = typeof params.name !== 'undefined' ? params.name : '';
	params.year = typeof params.year !== 'undefined' ? params.year : '';
	params.sheetType = typeof params.sheetType !== 'undefined' ? params.sheetType : '';
	params.role = typeof params.role !== 'undefined' ? params.role : '';
	params.progressType = params.progressType === 'review' ? 'review' : 'goal';
	params.division = params.division !== 'undefined' ? params.division : '';  
	var card = '<div class="eight wide column"><div class="ui card employee_card employee_id_' + params.userId + '"><div class="content">';
	//card += '<a href="/"><img class="right floated mini ui circular image" src="/img/user.png" alt="profile picture"></a>';
	card += '<div class="header performance_sheet_card_name">' + params.name + '</div>';
	card += '<div class="meta">';
	card += '<p class="sheet_type">' + params.sheetType + ' - ' + params.year + '</p>';
	card += '<p class="job_title">' + params.role + '</p>';
	card += '<p class="division_name">' + params.division + '</p>';
	card += '</div></div>';
	card += '<div class="extra content"><div class="ui grid"><div class="row"><div class="eleven wide column">';
	card += '<div class="ui equal width grid ' + params.progressType + '_progress_bar sheet_progress_bar">';
	card += '<div class="column"><div class="' + params.progressType + '_progress" title="viewing status" alt="progress"></div></div>';
	card += '<div class="column"><div class="' + params.progressType + '_progress" title="viewing status" alt="progress"></div></div>';
	card += '<div class="column"><div class="' + params.progressType + '_progress" title="viewing status" alt="progress"></div></div>';
	card += '<div class="column"><div class="' + params.progressType + '_progress" title="viewing status" alt="progress"></div></div>';
	card += '</div></div><div class="five wide column center aligned">';
	card += '<button class="ui small button options" data-sheetId="' + params.sheetId + '">Details</button>';
	card += '</div></div></div></div></div></div>';
	return $(card);
}

// Place properties from an object into a parent as hidden inputs
function createPropertyInputs(object, parent) {
	for (var prop in object) {
		if (object.hasOwnProperty(prop)) {
			if (typeof object[prop] !== 'object'
				&& typeof object[prop] !== 'string') {
				var input = $('<input type="hidden" />').prependTo(parent);
				input.attr('name', prop);
				input.val(object[prop]);
			}
		}
	}
}

function reserializeSheetItemsRecursive(params) {
	params.items = typeof params.items !== 'undefined' ? params.items : null;
	params.parent = typeof params.parent !== 'undefined' ? params.parent : null;
	params.itemName = typeof params.itemName !== 'undefined' ? params.itemName : 'performanceGoalSheetItems';
	params.itemTextName = typeof params.itemTextName !== 'undefined' ? params.itemTextName : 'performanceGoalSheetItemTexts';
	return params.items.map(function () {
		if (params.parent === null || $(this).parents('[data-serialize]').first().is(params.parent)) {
			var obj = { };
			var item = $(this);
			item.find(':input:not(button)').firstDataChildren(item).each(function () {
				if ($(this).attr('name').startsWith('performanceScoreId_')) {
					if ($(this).prop('checked')) {
						obj['performanceScoreId'] = $(this).val();
					}
				} else {
					if ($(this).attr('name') === 'weight') {
						obj[$(this).attr('name')] = parseFloat($(this).val()) / 100;
					}
					else {
						obj[$(this).attr('name')] = $(this).val();
					}
				}
			});
			var items = item.find('[data-serialize=true]').firstDataChildrenNonText(item);
			var texts = item.find('[data-serialize=true]').firstDataChildrenText(item);
			if (items.length > 0) {
				obj[params.itemName] = reserializeSheetItemsRecursive({ items: items, parent: item, itemName: params.itemName, itemTextName: params.itemTextName });
			}
			if (texts.length > 0) {
				obj[params.itemTextName] = reserializeSheetItemsRecursive({ items: texts, parent: item, itemName: params.itemName, itemTextName: params.itemTextName });
			}
			return obj;
		}
	}).get();
}

function approveButtons(approvalType, approvalText, unapprovalType, unapprovalText) {
	var data = { };
	if (typeof sheet.performanceReviewSheetItems !== 'undefined') {
		data.performanceReviewSheetId = sheet.performanceReviewSheetId;
	} else {
		data.performanceGoalSheetId = sheet.performanceGoalSheetId;
	}
	if (approvalText !== '') {
		$('.approve_button').text(approvalText).show().on('click', function () {
			var ready = true;
			//Reset Errors
			$('.comment_error, .score_error, .tab_error, sup.local_error').text('');
			$('.comment_error, .score_error, .tab_error, sup.local_error').remove();
			$('.local_error').removeClass('local_error');
			
			if (typeof sheet.performanceReviewSheetItems !== 'undefined') {
			// Review Sheet approve button click action functions
				$('.performance_item[data-serialize=true]').filter(function (i, parent) {
					return $(this).find('.performance_score').filter(function () {
						return $(this).dataParent()[0] === parent;
					}).length > 0;
				}).each(function (i, parent) {
					var field = findField($(this).find('[name="performanceFieldId"]').val());
					var tabn;
					var score = findScore($(this).find('.performance_score').filter(function () {return $(this).dataParent()[0] === parent;}).find(':input[type=radio]:checked').val());
					$(parent).children('.comment_error, score_error').remove();
					if (typeof score !== 'undefined' && score.needsComment) {
					//Add warnings under text items.
						if ($(parent).find('.performance_text textarea').val() === '') {
							$(parent).find('.performance_text').addClass('local_error').append('<sub class="comment_error">Please provide a <strong>reason</strong> for this score.</sub >');
							tabn = $(parent).dataParent().attr('data-tab');
							if ($('.tabular.menu div.item[data-tab=' + tabn + '] h2 sup').length === 0) {
								$('.tabular.menu div.item[data-tab=' + tabn + '] h2').append('<sup class="local_error tab_error">!</sup>');
							}
							ready = false;
						}
					}
					if (field.scored) {
					//Adding circles on the tab
						if (typeof score === 'undefined') {
							$(parent).find('.performance_score').addClass('local_error').append('<sub class="score_error">Please provide a score.</sub >');
							tabn = $(parent).dataParent().attr('data-tab');
							if ($('.tabular.menu div.item[data-tab=' + tabn + '] h2 sup').length === 0) {
								$('.tabular.menu div.item[data-tab=' + tabn + '] h2').append('<sup class="local_error tab_error">!</sup>');
							}
							ready = false;
						}
					}
				});
			}

			if (typeof sheet.performanceGoalSheetItems !== 'undefined') {
			// Goal Sheet checks
				var items = [$('.performance_item[data-serialize=true]')];
				var tab;
				$.each(items[0], function (index, item) {
					if ($(item).find('.performance_due_date').length !== 0) {
						if (!$(item).find('.performance_due_date input').val()) {
							$(item).find('.performance_due_date').append('<sub class="tab_error">Please provide a <strong>date</strong> of completion.</sub>').addClass('local_error');
							tab = $(item).parents('.ui.attached.tab').attr('data-tab');
							$('body .ui.attached.tabular.menu div[data-tab="' + tab + '"] h2').append('<sup class="local_error tab_error">!</sup>');
							ready = false;
						}
					}
					if ($(item).find('.field textarea').length !== 0) {
						if ($(item).find('.field textarea').val() === '') {
							if ($(item).find('.performance_text').length > 1) {
								var goalParent = $(item).find('.performance_text')[0];
								$(goalParent).append('<sub class="tab_error">Please provide a goal or explanation.</sub>').addClass('local_error');
								ready = false;
							} else {
								$(item).find('.performance_text').append('<sub class="tab_error">Please provide a goal or explanation.</sub>').addClass('local_error');
								var accordionTab = $(item).parents('.ui.fluid.accordion.item_detail').children('.title').append('<sup class="local_error tab_error">!</sup>');
								ready = false;
							}
							tab = $(item).parents('.ui.attached.tab').attr('data-tab');
							$('body .ui.attached.tabular.menu div[data-tab="' + tab + '"] h2').append('<sup class="local_error">!</sup>');
							ready = false;
						}
					}
				});

				//checking sheet year because 2020 has weights, while 2019 does not.
				if (sheet.year > 2019) {
					if (!checkWeights()) {
						ready = false;
					}
				}
			}

			// since sheet is ready, saveSheet then approve.
			if (ready) {
				if (confirm('Are you sure you want to approve this version of the sheet? This will move it upwards in the process.')) {
					saveApprove(sheet, approvalType);
				}
			} else {
				alert('Please review your sheet for any errors.');
			}
		});
	} else {
		$('.approve_button').remove();
	}
	if (unapprovalText !== '') {
		$('.unapprove_button').text(unapprovalText).show().on('click', function () {
			if (confirm('Are you sure you want to reset your approval of this sheet? This will reset the sheet\'s status and allow you to edit your inputs.')) {
				data.approved = false;
				data.approvalType = 'Employee';
				data.userId = sheet.userId;
				if (typeof sheet.performanceReviewSheetItems !== 'undefined') {
					data.performanceReviewSheetId = sheet.performanceReviewSheetId;
				} else {
					data.performanceGoalSheetId = sheet.performanceGoalSheetId;
				}
				ajSubmit({
					data: JSON.stringify(data),
					url: '/api/Performance/' + (typeof sheet.performanceReviewSheetItems !== 'undefined' ? 'Review' : 'Goal') + 'Sheet/Approve',
					workingMessage: 'Revoking sheet status approval...',
					successMessage: 'Reset complete.',
					successClear: false,
					successFunction: function (data) {
						location.reload();
					},
					successFunctionWait: 3000,
					failedMessage: 'Failed to update sheet.'
				});
			}
		});
	} else {
		$('.unapprove_button').remove();
	}
}

function sheetButtons(params) {
	params.self = typeof params.self !== 'undefined' ? params.self : false;
	params.supervisor = typeof params.supervisor !== 'undefined' ? params.supervisor : false;
	params.lead = typeof params.lead !== 'undefined' ? params.lead : false;
	params.administrator = typeof params.administrator !== 'undefined' ? params.administrator : false;
	params.employeeComplete = typeof params.employeeComplete !== 'undefined' ? params.employeeComplete : false;
	params.managerComplete = typeof params.managerComplete !== 'undefined' ? params.managerComplete : false;
	params.leadComplete = typeof params.leadComplete !== 'undefined' ? params.leadComplete : false;
	params.hrComplete = typeof params.hrComplete !== 'undefined' ? params.hrComplete : false;
	params.reviewSheet = typeof params.reviewSheet !== 'undefined' ? true : false;
	params.goalSheet = typeof params.goalSheet !== 'undefined' ? true : false;
	params.reviewActive = typeof params.reviewActive !== 'undefined' ? true : false;

	$('.sheet_sub_nav, .local_spacer').slideDown();
	$('.expander').show();

	// Remove the save button in all instances except when the employee has yet to approve the sheet.
	if (!params.self || sheet.employeeComplete) {
		$('.save_button').remove();
	}

	// Remove the entire bar when you are only a viewer of the sheet
	if (!params.self && !params.supervisor && !params.lead && !params.administrator) {
		approveButtons('', '', '', '');
	}

	// Filters sheet status versus permissions to show or hide the approval buttons.
	if (sheet.hrComplete) {
		if ((params.self || params.administrator) && params.reviewActive === false && params.goalSheet === true) {
			approveButtons('', '', 'Employee', 'Take Back for Edits');
		} else {
			$('.approve_button').remove();
			$('.unapprove_button').remove();
		}
	} else if (params.leadComplete) {
		if (params.self && params.administrator) {
			approveButtons('HR', 'Final Approval', 'Employee', 'Take Back for Edits');
		} else if (!params.self && params.administrator) {
			approveButtons('HR', 'Final Approval', 'Employee', 'Send Back to Employee');
		} else {
			if (params.self) {
				approveButtons('', '', 'Employee', 'Take Back for Edits');
			} else {
				approveButtons('', '', '', '');
			}
		}
	} else if (params.managerComplete) {
		if (!params.self && (params.lead || params.administrator)) {
			approveButtons('Lead', 'Approve (by lead)', 'Employee', 'Send Back to Employee');
		} else {
			if (params.self) {
				approveButtons('', '', 'Employee', 'Take Back for Edits');
			} else {
				approveButtons('', '', '', '');
			}
		}
	} else if (params.employeeComplete) {
		if (!params.self && (params.supervisor || params.administrator)) {
			approveButtons('Manager', 'Approve (by manager)', 'Employee', 'Send Back to Employee');
		} else {
			if (params.self) {
				approveButtons('', '', 'Employee', 'Take Back for Edits');
			} else {
				approveButtons('', '', '', '');
			}
		}
	} else if (params.self) {
		approveButtons('Employee', 'Send to Manager', '', '');
	} else {
		approveButtons('', '', '', '');
	}
}

function sortItems() {
	return this.sort(function (a, b) {
		if (typeof a.itemOrder !== 'undefined') {
			return a.itemOrder - b.itemOrder;
		}
		return a.displayOrder - b.displayOrder;
	});
}

Array.prototype.sortItems = sortItems;

function buildTabs(params) {
	params.tabs = typeof params.tabs !== 'undefined' ? params.tabs : null;
	params.divisionId = typeof params.divisionId !== 'undefined' ? params.divisionId : null;
	params.self = typeof params.self !== 'undefined' ? params.self : false;
	params.supervisor = typeof params.supervisor !== 'undefined' ? params.supervisor : false;
	params.lead = typeof params.lead !== 'undefined' ? params.lead : false;
	params.administrator = typeof params.administrator !== 'undefined' ? params.administrator : false;
	params.reviewActive = typeof params.reviewActive !== 'undefined' ? params.reviewActive : false;
	params.userId = typeof params.userId !== 'undefined' ? params.userId : null;
	params.sheetType = typeof params.sheetType !== 'undefined' ? params.sheetType : null;

	loadLookups({
		callback: function () {
			$.each(params.tabs.sortItems(), function (i, tab) {

				var header = $('#tab_header').clone();
				header.css('display', 'block');
				header.children().first().text(findField(tab.performanceFieldId).name);
				header.attr('id', 'tab-' + tab.performanceFieldId);
				header.attr('data-tab', createPropertyName(findField(tab.performanceFieldId).name));

				$('#tab_header').parent().append(header);

				var body = $('#tab_body').clone();
				body.css("display", "");
				body.attr('data-serialize', true);
				body.attr('id', 'item-' + tab.performanceFieldId);
				body.attr('data-tab', createPropertyName(findField(tab.performanceFieldId).name));
				body.find('.show_for_print').text(findField(tab.performanceFieldId).name);

				$('#tab_body').parent().append(body);

				createPropertyInputs(tab, body);

				// Sets active tabs headers and tab body on sheets
				if (params.sheetType !== "goal" && i === 0) {
					header.addClass('active');
					body.addClass('active');
				}

				// Handling nested children
				if (typeof tab.performanceReviewSheetItems !== 'undefined') {
					buildItems({
						items: tab.performanceReviewSheetItems,
						userId: params.userId,
						parent: body,
						divisionId: params.divisionId,
						self: params.self,
						supervisor: params.supervisor,
						lead: params.lead,
						administrator: params.administrator,
						reviewActive: params.reviewActive
					});
				} else if (typeof tab.performanceGoalSheetItems !== 'undefined') {
					if (findField(tab.performanceGoalSheetItems[0].performanceFieldId).allowMultiple) {
						buildItems({
							items: [templateify(tab.performanceGoalSheetItems[0])],
							parent: body,
							userId: params.userId,
							serialize: false,
							idPrefix: 'template' + tab.performanceGoalSheetItems[0].performanceFieldId + '-',
							self: params.self,
							supervisor: params.supervisor,
							lead: params.lead,
							administrator: params.administrator,
							reviewActive: params.reviewActive
						});
					}
					buildItems({
						items: tab.performanceGoalSheetItems,
						parent: body,
						userId: params.userId,
						divisionId: params.divisionId,
						self: params.self,
						supervisor: params.supervisor,
						lead: params.lead,
						administrator: params.administrator,
						reviewActive: params.reviewActive
					});
				}
				$('.menu .item').tab();
			});
		}
	});
}

// Build all texts for items
function buildTexts(params) {
	params.texts = typeof params.texts !== 'undefined' ? params.texts : null;
	params.parent = typeof params.parent !== 'undefined' ? params.parent : null;
	params.serialize = typeof params.serialize !== 'undefined' ? params.serialize : true;
	params.idPrefix = typeof params.idPrefix !== 'undefined' ? params.idPrefix : '';
	params.self = typeof params.self !== 'undefined' ? params.self : false;
	params.supervisor = typeof params.supervisor !== 'undefined' ? params.supervisor : false;
	params.lead = typeof params.lead !== 'undefined' ? params.lead : false;
	params.administrator = typeof params.administrator !== 'undefined' ? params.administrator : false;
	params.reviewActive = typeof params.reviewActive !== 'undefined' ? params.reviewActive : false;
	params.readOnly = typeof params.readOnly !== 'undefined' ? params.readOnly : false;
	params.pfield = typeof params.pfield !== 'undefined' ? params.pfield : null;

	var readOnly = params.readOnly ? ' readonly' : '';
	$.each(params.texts, function (i, text) {
		if (findTextType(text.performanceTextTypeId).name === 'Standard') {
			var new_item = $('<div class="field"></div>').appendTo(params.parent);
			new_item.attr('data-serialize', params.serialize);
			createPropertyInputs(text, new_item);
			$('<label for=""></label><textarea class="hide_for_print" rows="2" placeholder="Explanation:"' + readOnly + '></textarea><div class="show_for_print item_text_copy" style="white-space: pre-wrap;"></div>').appendTo(new_item);
			var id = 'text-' + params.idPrefix;
			if (typeof text.performanceGoalSheetItemIdTemp !== 'undefined') {
				id += '-' + text.performanceTextTypeId;
			} else if (typeof text.performanceGoalSheetItemId !== 'undefined') {
				id += text.performanceGoalSheetItemId + '-' + text.performanceTextTypeId;
			} else {
				id += text.performanceReviewSheetItemId + '-' + text.performanceTextTypeId;
			}
			new_item.children('textarea').attr('id', id);
			new_item.children('textarea').attr('name', 'contents');
			new_item.children('label').attr('for', id);
			new_item.children('textarea').val(text.contents);
			new_item.children('textarea').siblings('.item_text_copy').text(new_item.children('textarea').val());
			if (!params.readOnly) {
				new_item.children('label').text('Employee Input:');
			} else {
				new_item.children('textarea').hide();
				new_item.children('textarea').siblings('.item_text_copy').removeClass('show_for_print');
				if (new_item.parent().siblings(".performance_header").text() == "Current ATG University Level") {
					var trainingLevels = {
						"Learning A": "<ul><li>Basic Onboarding</li><li>Project Management Plan Fundamentals</li><li>QA/QC Intro</li><li>Schedules Intro</li><li>Consulting 101</li><li>Time Management</li><li>Interpersonal Communication</li><li>Goal Setting</li><li>Performance Review</li><li>Technical Trainings (as assigned by Supervisor)</li></ul>",
						"Learning B": "<ul><li>Technical Trainings (as assigned by Supervisor)</li><li>ATGLD/MS Project 100</li><li>Project Management Basics 100 (CPM, PMB and EV)</li><li>WBS Development 100</li><li>Teamwork</li><li>Leadership</li></ul>",
						"Teaching A": "<ul><li>Technical Trainings (as assigned by Supervisor)</li><li>QA/QC 200</li><li>Project Management Basics 200 (Intermediate CPM, PMB and EV)</li><li>Developing Scope (WBS) 100</li><li>EV Management 200</li><li>MS Project 200</li><li>Advanced Communication</li><li>Marketing SOP</li><li>Contract Agreements, incl. Liability and Risk Mgmt</li><li>On-Call Contracts: BD, Management and WAs</li></ul>",
						"Teaching B": "<ul><li>Technical Trainings (as assigned by Supervisor)</li><li>MS Project Advanced, incl. Resource Allocations</li><li>EV Application, PM Dashboard Setup</li><li>Project Management Plan Development and Mgmt</li><li>Leading the Project Team, incl Defining Roles in the Plan</li><li>Communication Techniques and Plan</li><li>Project Quality Plan</li><li>Project Recovery Plan</li><li>Project Kickoff (Everything is ready Day 0)</li><li>Managing Client Relationships</li><li>Business Development</li><li>Business Development SOP</li></ul>",
						"Leadership A": "Discussion with supervisor to determine Strategic Planning, People Management, Goal Setting, and/or other enterprise focused classes.",
						"Leadership B": "Discussion with supervisor to determine Strategic Planning, People Management, Goal Setting, and/or other enterprise focused classes.",
						"Executive A": "Discussion with supervisor to determine Strategic Planning, People Management, Goal Setting, and/or other enterprise focused classes.",
						"Executive B": "Discussion with supervisor to determine Strategic Planning, People Management, Goal Setting, and/or other enterprise focused classes.",
						"Executive C": "Discussion with supervisor to determine Strategic Planning, People Management, Goal Setting, and/or other enterprise focused classes."
					}
					new_item.children('textarea').siblings('.item_text_copy').html("<p class=\"bold\">" + text.contents + " -</p>" + trainingLevels[text.contents]);
				}
				//console.log(new_item.parent()).text();
			}

			new_item.children('textarea').on('input', function () {
				$(this).siblings('.item_text_copy').text($(this).val());
			});

			// Readonly text conditions for goal sheet.
			if (text.performanceGoalSheetItemId) {
				if (sheet.employeeComplete || !params.self || params.reviewActive) {
					new_item.children('textarea').addClass('disabled').attr('readonly', 'readonly');
					new_item.parents('.performance_item').find('.performance_due_date input').addClass('disabled').attr('readonly', 'readonly');
					new_item.parents('.performance_item').find('.performance_weight input').addClass('disabled').attr('readonly', 'readonly');
				}
			}

			// Readonly text conditions for review sheet.
			if (text.performanceReviewSheetItemId) {
				if (sheet.employeeComplete || !params.self) {
					new_item.children('textarea').addClass('disabled').attr('readonly', 'readonly');
					new_item.parents('.performance_item').find('.ui.radio.checkbox').addClass('disabled').attr("disabled", true).attr('readonly', true);				}
			}
		}
	});
}

// Build all texts for items
function buildComments(params) {
	params.texts = typeof params.texts !== 'undefined' ? params.texts : null;
	params.parent = typeof params.parent !== 'undefined' ? params.parent : null;
	params.serialize = false; // Comments will not be serialized into sheet updates.
	params.idPrefix = typeof params.idPrefix !== 'undefined' ? params.idPrefix : '';
	params.self = typeof params.self !== 'undefined' ? params.self : false;
	params.supervisor = typeof params.supervisor !== 'undefined' ? params.supervisor : false;
	params.lead = typeof params.lead !== 'undefined' ? params.lead : false;
	params.administrator = typeof params.administrator !== 'undefined' ? params.administrator : false;
	params.userId = typeof params.userId !== 'undefined' ? params.userId : null;

	$.each(params.texts, function (i, text) {
		if (findTextType(text.performanceTextTypeId).name === 'Comment') {
			params.parent.addClass('active');
			params.parent.siblings('.title').addClass('active');
			params.parent.parents('.performance_comment_top').find('.comment_count').text(function () { return parseInt($(this).text()) + 1; });

			// Will expand comment body if there are comments.
			if (parseInt($(this).text()) + 1 > 0) {
				params.parent.parents('.performance_comment_top').find('.comment_box .content').addClass('active');
			}

			var new_comment = $('#comment_template').clone().appendTo(params.parent).attr('id', '').show();
			new_comment.find('.c_owner').text(findUser(text.userId).name);
			new_comment.find('.c_timestamp').text($.format.date(text.dataDatetime, 'MM/dd/yyyy hh:mm a'));
			new_comment.find('.c_text').text(text.contents);
			new_comment.find('.c_textinput').val(text.contents);
			new_comment.attr('performanceItemTextId', typeof text.performanceGoalSheetItemTextId !== 'undefined' ? text.performanceGoalSheetItemTextId : text.performanceReviewSheetItemTextId);
			if (params.userId !== text.userId
				|| text.contents === '[Deleted]'
				|| typeof (typeof text.performanceGoalSheetItemTextId !== 'undefined' ? text.performanceGoalSheetItemTextId : text.performanceReviewSheetItemTextId) === 'undefined'
				|| (typeof text.performanceGoalSheetItemTextId !== 'undefined' ? text.performanceGoalSheetItemTextId : text.performanceReviewSheetItemTextId) === '') {
				new_comment.find('.c_delete,.c_edit,.c_textinput,.hide_for_print').remove();
				if (typeof (typeof text.performanceGoalSheetItemTextId !== 'undefined' ? text.performanceGoalSheetItemTextId : text.performanceReviewSheetItemTextId) === 'undefined'
					|| (typeof text.performanceGoalSheetItemTextId !== 'undefined' ? text.performanceGoalSheetItemTextId : text.performanceReviewSheetItemTextId) === '') {
					new_comment.find('.c_reply').parents().eq(1).remove();
				}
			}

			if (text.performanceGoalSheetItemTexts !== undefined) {
				new_comment.find('.performance_comment_replies').show();
				buildComments({
					texts: text.performanceGoalSheetItemTexts,
					parent: new_comment.find('.performance_comment_replies').find('.content'),
					serialize: false,
					idPrefix: params.idPrefix,
					self: params.self,
					supervisor: params.supervisor,
					lead: params.lead,
					administrator: params.administrator,
					userId: params.userId
				});
			} else if (text.performanceReviewSheetItemTexts !== undefined) {
				new_comment.find('.performance_comment_replies').show();
				buildComments({
					texts: text.performanceReviewSheetItemTexts,
					parent: new_comment.find('.performance_comment_replies').find('.content'),
					serialize: false,
					idPrefix: params.idPrefix,
					self: params.self,
					supervisor: params.supervisor,
					lead: params.lead,
					administrator: params.administrator,
					userId: params.userId
				});
			}
		}
	});
}

function accordionCleaner() {
	$('.ui.accordion').each(function (index) {
		if ($(this).parents('.ui.accordion').length > 0) {
			$(this).removeClass('ui');
		}
	});
	$('.ui.accordion').accordion();
}

function commentSave(comment, params) {
	params.items = typeof params.items !== 'undefined' ? params.items : null;
	params.parent = typeof params.parent !== 'undefined' ? params.parent : null;
	params.divisionId = typeof params.divisionId !== 'undefined' ? params.divisionId : null;
	params.serialize = typeof params.serialize !== 'undefined' ? params.serialize : true;
	params.idPrefix = typeof params.idPrefix !== 'undefined' ? params.idPrefix : '';
	params.self = typeof params.self !== 'undefined' ? params.self : false;
	params.supervisor = typeof params.supervisor !== 'undefined' ? params.supervisor : false;
	params.lead = typeof params.lead !== 'undefined' ? params.lead : false;
	params.administrator = typeof params.administrator !== 'undefined' ? params.administrator : false;
	params.userId = typeof params.userId !== 'undefined' ? params.userId : null;

	if (comment.length > 0 && comment.val() !== '') {
		commentObject = {
			'performanceTextTypeId': findTextTypeByName('Comment').performanceTextTypeId,
			'contents': comment.val(),
			'userId': params.userId
		};
		commentObject['performance' + (typeof sheet.performanceReviewSheetItems !== 'undefined' ? 'Review' : 'Goal') + 'SheetItemId'] = comment.parents('[performanceItemId]').first().attr('performanceItemId');
		commentObject['performance' + (typeof sheet.performanceReviewSheetItems !== 'undefined' ? 'Review' : 'Goal') + 'SheetItemTextId'] = comment.parents('[performanceItemTextId]').first().attr('performanceItemTextId').length > 0 ? comment.parents('[performanceItemTextId]').first().attr('performanceItemTextId') : null;
		commentObject['parentPerformance' + (typeof sheet.performanceReviewSheetItems !== 'undefined' ? 'Review' : 'Goal') + 'SheetItemTextId'] = comment.parents('[performanceItemTextId]').eq(1).length > 0 ? comment.parents('[performanceItemTextId]').eq(1).attr('performanceItemTextId') : null;
		ajSubmit({
			data: JSON.stringify(commentObject),
			url: '/api/Performance/' + (typeof sheet.performanceReviewSheetItems !== 'undefined' ? 'Review' : 'Goal') + 'Sheet/Comment',
			workingMessage: 'Saving comment...',
			successMessage: 'Save complete.',
			successClear: true,
			successFunction: function (data) {
				if (typeof (typeof commentObject.performanceGoalSheetItemTextId !== 'undefined' ? commentObject.performanceGoalSheetItemTextId : commentObject.performanceReviewSheetItemTextId) === 'undefined'
					|| (typeof commentObject.performanceGoalSheetItemTextId !== 'undefined' ? commentObject.performanceGoalSheetItemTextId : commentObject.performanceReviewSheetItemTextId) === '') {
					commentObject['dataDatetime'] = Date.now();
					var parent = comment.parents().eq(2).children().first().is(comment.parents().eq(1)) ? comment.parents().eq(2) : comment.parents().eq(1).siblings().last();
					parent.show();
					buildComments({
						texts: [ commentObject ],
						parent: parent,
						serialize: false,
						idPrefix: params.idPrefix,
						self: params.self,
						supervisor: params.supervisor,
						lead: params.lead,
						administrator: params.administrator,
						userId: params.userId
					});
					comment.val('');
					comment.parents('.content').first().removeClass('active').siblings('.title').removeClass('active');
					comment.parents('.accordion').first().removeClass('active');
					commentEvents({
						parent: parent,
						serialize: false,
						idPrefix: params.idPrefix,
						self: params.self,
						supervisor: params.supervisor,
						lead: params.lead,
						administrator: params.administrator,
						reviewActive: params.reviewActive,
						userId: params.userId
					});
				} else {
					var button = comment.parent().siblings('.c_edit');
					button.siblings('.c_edit').html('Edit &nbsp;<i class="eraser icon"></i>');
					button.siblings('.c_text').text(button.siblings('.c_textinput').find('textarea.comment_text').val());
					button.siblings('.c_textinput').hide();
					button.siblings('.c_text').show();
				}
			},
			successFunctionWait: 3000,
			failedMessage: 'Failed to save comment.'
		});
	}
}

function commentEvents(params) {
	params.items = typeof params.items !== 'undefined' ? params.items : null;
	params.parent = typeof params.parent !== 'undefined' ? params.parent : null;
	params.divisionId = typeof params.divisionId !== 'undefined' ? params.divisionId : null;
	params.serialize = typeof params.serialize !== 'undefined' ? params.serialize : true;
	params.idPrefix = typeof params.idPrefix !== 'undefined' ? params.idPrefix : '';
	params.self = typeof params.self !== 'undefined' ? params.self : false;
	params.supervisor = typeof params.supervisor !== 'undefined' ? params.supervisor : false;
	params.lead = typeof params.lead !== 'undefined' ? params.lead : false;
	params.administrator = typeof params.administrator !== 'undefined' ? params.administrator : false;
	params.userId = typeof params.userId !== 'undefined' ? params.userId : null;

	params.parent.find('.c_edit').off('click select').on('click select', function (event) {
		switch ($(this).siblings('.c_text').css('display')) {
			case 'none':
				$(this).html('Edit &nbsp;<i class="eraser icon"></i>');
				$(this).siblings('.c_textinput').hide();
				$(this).siblings('.c_text').show();
				break;
			default:
				$(this).html('Cancel &nbsp;<i class="eraser icon"></i>');
				$(this).siblings('.c_textinput').find('textarea.comment_text').val($(this).siblings('.c_text').text());
				$(this).siblings('.c_textinput').show();
				$(this).siblings('.c_text').hide();
				break;
		}
	});
	params.parent.find('.c_save').off('click select').on('click select', function (event) {
		commentSave(
			$(this).siblings('textarea.comment_text'),
			{
				parent: params.parent,
				serialize: false,
				idPrefix: params.idPrefix,
				self: params.self,
				supervisor: params.supervisor,
				lead: params.lead,
				administrator: params.administrator,
				reviewActive: params.reviewActive,
				userId: params.userId
		});
	});
	params.parent.find('.c_delete').off('click select').on('click select', function (event) {
		if (confirm('Are you sure you want to delete this comment? It will remain as an object on the sheet, but its contents will be emptied.')) {
			var parent = $(this).parents('[performanceItemTextId]').first();
			ajSubmit({
				url: '/api/Performance/' + (typeof sheet.performanceReviewSheetItems !== 'undefined' ? 'Review' : 'Goal') + 'Sheet/Comment/' + parent.attr('performanceItemTextId') + '/Delete',
				workingMessage: 'Deleting comment...',
				successMessage: 'Delete complete.',
				successClear: true,
				successFunction: function (data) {
					parent.children('.c_text').text('[Deleted]');
					parent.children('.c_delete,.c_edit,.c_textinput,.hide_for_print').remove();
				},
				successFunctionWait: 3000,
				failedMessage: 'Failed to delete comment.'
			});
		}
	});
}

function buildItems(params) {
	params.items = typeof params.items !== 'undefined' ? params.items : null;
	params.parent = typeof params.parent !== 'undefined' ? params.parent : null;
	params.divisionId = typeof params.divisionId !== 'undefined' ? params.divisionId : null;
	params.serialize = typeof params.serialize !== 'undefined' ? params.serialize : true;
	params.idPrefix = typeof params.idPrefix !== 'undefined' ? params.idPrefix : '';
	params.self = typeof params.self !== 'undefined' ? params.self : false;
	params.supervisor = typeof params.supervisor !== 'undefined' ? params.supervisor : false;
	params.lead = typeof params.lead !== 'undefined' ? params.lead : false;
	params.administrator = typeof params.administrator !== 'undefined' ? params.administrator : false;
	params.userId = typeof params.userId !== 'undefined' ? params.userId : null;
	params.reviewActive = typeof params.reviewActive !== 'undefined' ? params.reviewActive : null;

	$.each(params.items.sortItems(), function (i, item) {
		var id = 'item-' + params.idPrefix;

		if (typeof item.performanceGoalSheetItemIdTemp !== 'undefined') {
			id += item.performanceFieldId;
		} else if (typeof item.performanceGoalSheetItemId !== 'undefined') {
			id += item.performanceGoalSheetItemId;
		} else if (typeof item.performanceReviewSheetItemId !== 'undefined') {
			id += item.performanceReviewSheetItemId;
		}

		var pfieldMaster = ["A/R", "Job Starts", "Multiplier", "MUT", "Personal UT goal, applicable Job Start goal, multiplier goal, etc.", "Revenues", "Utilization", "Goal"]
		var pfield = findField(item.performanceFieldId);
		var new_item;
		if (item.past && (pfield.name === 'Goal' || pfield.name === 'Utilization')) {
			var parentAccodionContent = $("#" + id + "[pastparent='true']").find('#PastAccordionContent');
			new_item = $('#item_template').clone().attr('id', id).attr('data-serialize', params.serialize).appendTo(parentAccodionContent);
		} else {
			new_item = $('#item_template').clone().attr('id', id).attr('data-serialize', params.serialize).attr('PastParent', true).appendTo(params.parent);
			if (!pfieldMaster.includes(pfield.name)) {
				new_item.find('#PastAccordion').remove();
			}
		}
		
		createPropertyInputs(item, new_item);
		// audit styling
		if (typeof item.past !== 'undefined' && item.past === true && item.deleted === false) {
			new_item.find('#top_level').addClass('dark_form_box');
			new_item.find('#top_level').removeClass('white_form_box');
			new_item.find('.comment_box').addClass('comment_box_past');
			new_item.find('.comment_box_past').removeClass('comment_box');
			new_item.find('.ui.fluid.accordion').removeClass('fluid');
			new_item.find('#PastAccordion').remove();
		} else if (typeof item.past !== 'undefined' && item.deleted === true) {
			new_item.find('#top_level').addClass('red_form_box');
			new_item.find('#top_level').removeClass('white_form_box');
			new_item.find('.comment_box').addClass('comment_box_past');
			new_item.find('.comment_box_past').removeClass('comment_box');
			if (item.past === true) {
				new_item.find('#PastAccordion').remove();
			}
		}

		// Item headers
		if (params.parent.hasClass('performance_children')) {
			new_item.find('.performance_children_header').html(pfield.name);
			new_item.find('.performance_children_header').css('display', 'block');
			new_item.find('.performance_header').remove();
		} else {
			new_item.find('.performance_header').text(pfield.name);
			new_item.find('.performance_header').css('display','block');
		}

		// Builds tooltip
		if (pfield.description !== '') {
			new_item.find('.performance_header').popup({
				hoverable: true,
				position: 'top left',
				variation: 'very wide',
				html : pfield.description,
				error: {
					invalidPosition : 'The position you specified is not a valid position',
					cannotPlace     : 'Popup does not fit within the boundaries of the viewport',
					method          : 'The method you called is not defined.',
					noTransition    : 'This module requires ui transitions <https: github.com="" semantic-org="" ui-transition="">',
					notFound        : 'The target or popup you specified does not exist on the page'
				}
			});
		}

		var division = typeof params.divisionId !== 'undefined' ? findDivision(params.divisionId) : null;
		var item_name = findField(item.performanceFieldId).name;

		// show/hide practice description on item if there is practice level data
		if (typeof division !== 'undefined'
			&& typeof division.parentDivisionId !== 'undefined'
			&& typeof findDescription(division.divisionId, findFieldByYearAndName(sheet.year, item_name).performanceFieldId) !== 'undefined'
			) {
			var prac_description = findDescription(division.divisionId, findFieldByYearAndName(sheet.year, item_name).performanceFieldId).description;
			new_item.find('.performance_practice_description #p_goal').text(prac_description);
			new_item.find('.performance_practice_description').css('display', 'block');
		} else {
			new_item.find('.performance_practice_description').remove();
		}

		// show/hide practice description on item if there is division level data
		if (typeof division !== 'undefined'
			&& typeof division.parentDivisionId !== 'undefined'
			&& typeof findDescription(division.parentDivisionId, item.performanceFieldId) !== 'undefined') {
			var div_description = findDescription(division.parentDivisionId, item.performanceFieldId);
			new_item.find('.performance_division_description #d_goal').text(div_description.description);
			new_item.find('.performance_division_description').css('display', 'block');
		}
		else if (typeof division !== 'undefined'
			&& typeof division.parentDivisionId === 'undefined'
			&& typeof findDescription(division.divisionId, item.performanceFieldId) !== 'undefined') {
			var div_description_main = findDescription(division.divisionId, item.performanceFieldId);
			new_item.find('.performance_division_description #d_goal').text(div_description_main.description);
			new_item.find('.performance_division_description').css('display', 'block');
		}
		else {
			new_item.find('.performance_division_description').remove();
		}

		if (pfield.date && typeof item.performanceGoalSheetId !== 'undefined') {
			new_item.find('.performance_top_form_row').css('display', 'block');
			new_item.find('.performance_due_date input').attr('id', 'dueDate-' + params.idPrefix + item.performanceFieldId);
			new_item.find('.performance_due_date label').attr('for', 'dueDate-' + params.idPrefix + item.performanceFieldId);
			var date;

			if (typeof item.performanceGoalSheetItemDate !== 'undefined') {
				date = new Date(item.performanceGoalSheetItemDate);
			} else if (typeof item.performanceReviewSheetItemDate !== 'undefined') {
				date = new Date(item.performanceReviewSheetItemDate);
			}
			new_item.find('.performance_due_date input').val(formatInputDate(date));
		} else {
			new_item.find('.performance_top_form_row').remove();
			new_item.find('.performance_due_date').remove();
		}

		//goal sheet weights
		if (typeof item.performanceGoalSheetId !== 'undefined' && pfield.employeeWeighted) {
			new_item.find('.performance_top_form_row').css('display', 'block');
			new_item.find('.performance_weight').css('display', 'block');
			new_item.find('.performance_weight input').val((item.weight * 100).toFixed(2));
			$('body .weight_label').popup();
		} else {
			new_item.find('.performance_weight').remove();
		}

		if (typeof item.performanceReviewSheetItemId !== 'undefined' && pfield.scored) {
			new_item.find('.performance_score').css('display', 'block');
			new_item.find('.performance_score :input')
				.attr('id', function (i, id) {
					return 'item_' + item.performanceReviewSheetItemId + '_score_' + params.idPrefix + id.match(/\d+$/g);
				})
				.attr('name', 'performanceScoreId_' + item.performanceReviewSheetItemId);
			new_item.find('.ui.checkbox').checkbox();
			new_item.find('.performance_score :input').firstDataChildren(new_item).each(function (i, score) {
				if (typeof item.performanceScoreId !== 'undefined' && item.performanceScoreId === parseInt($(this).val())) {
					$(this).prop('checked', true).change();
				}
			});
			new_item.find('.performance_score :input').firstDataChildren(new_item).each(function (i, input) {
				var score = findScore($(this).val());
				if (typeof score !== 'undefined' && typeof score.description !== 'undefined') {
					$(this).siblings('label').first().popup({
						hoverable: true,
						position: 'top center',
						variation: 'very wide',
						html: score.description,
						delay: {
							show: 800,
							hide: 0
						},
						error: {
							invalidPosition: 'The position you specified is not a valid position',
							cannotPlace: 'Popup does not fit within the boundaries of the viewport',
							method: 'The method you called is not defined.',
							noTransition: 'This module requires ui transitions <https: github.com="" semantic-org="" ui-transition="">',
							notFound: 'The target or popup you specified does not exist on the page'
						}
					});
				}
			});
		} else {
			new_item.find('.performance_score').remove();
		}

		if (pfield.performanceFieldSelectionGroupId) {

		}
		
		if (pfield.needsText) {
			new_item.find('.performance_text').css('display', 'block');
			if (typeof item.performanceGoalSheetItemTexts !== 'undefined' && item.performanceGoalSheetItemTexts.length > 0) {
				buildTexts({
					texts: item.performanceGoalSheetItemTexts,
					parent: new_item.find('.performance_text'),
					serialize: params.serialize,
					idPrefix: params.idPrefix + item.performanceFieldId,
					self: params.self,
					supervisor: params.supervisor,
					lead: params.lead,
					administrator: params.administrator,
					reviewActive: params.reviewActive,
					readOnly: pfield.goalsReadOnly,
					pfield: pfield
				});
			} else if (typeof item.performanceReviewSheetItemTexts !== 'undefined' && item.performanceReviewSheetItemTexts.length > 0) {
				buildTexts({
					texts: item.performanceReviewSheetItemTexts,
					parent: new_item.find('.performance_text'),
					serialize: params.serialize,
					idPrefix: params.idPrefix + item.performanceFieldId,
					self: params.self,
					supervisor: params.supervisor,
					lead: params.lead,
					administrator: params.administrator,
					reviewActive: params.reviewActive,
					readOnly: pfield.reviewReadOnly,
					pfield: pfield
				});
			}
		} else {
			new_item.find('.performance_text').remove();
		}

		if (pfield.allowComment) {
			new_item.find('.performance_comment').css('display', 'block');
			if (typeof item.performanceGoalSheetItemTexts !== 'undefined' && item.performanceGoalSheetItemTexts.length > 0) {
				buildComments({
					texts: item.performanceGoalSheetItemTexts,
					parent: new_item.find('.performance_comment .content').first(),
					serialize: false,
					idPrefix: params.idPrefix + item.performanceFieldId,
					self: params.self,
					supervisor: params.supervisor,
					lead: params.lead,
					administrator: params.administrator,
					reviewActive: params.reviewActive,
					userId: params.userId
				});
			} else if (typeof item.performanceReviewSheetItemTexts !== 'undefined' && item.performanceReviewSheetItemTexts.length > 0) {
				buildComments({
					texts: item.performanceReviewSheetItemTexts,
					parent: new_item.find('.performance_comment .content').first(),
					serialize: false,
					idPrefix: params.idPrefix + item.performanceFieldId,
					self: params.self,
					supervisor: params.supervisor,
					lead: params.lead,
					administrator: params.administrator,
					reviewActive: params.reviewActive,
					userId: params.userId
				});
			}

			commentEvents({
				parent: new_item,
				serialize: false,
				idPrefix: params.idPrefix + item.performanceFieldId,
				self: params.self,
				supervisor: params.supervisor,
				lead: params.lead,
				administrator: params.administrator,
				reviewActive: params.reviewActive,
				userId: params.userId
			});
			new_item.find('[performanceItemTextId]').attr('performanceItemId', typeof item.performanceReviewSheetItemId !== 'undefined' ? item.performanceReviewSheetItemId : item.performanceGoalSheetItemId);
			$('#reply_comment_template').clone().appendTo(new_item.find('.performance_comment .content')).attr('id', '').show();
		} else {
			new_item.find('.performance_comment').remove();
		}

		if (pfield.goals && pfield.allowMultiple) {
			//sets label as the goal sheet's goal text for both goal sheet and review sheet
			if (typeof item.performanceGoalSheetItemId !== 'undefined') {
				// builds delete button for goal sheets only
				new_item.find('.performance_delete_button').css('display', 'block');
				new_item.find('.performance_delete_button button').attr('id', 'delete_item_' + params.idPrefix + (typeof item.performanceGoalSheetItemIdTemp !== 'undefined' ? item.performanceFieldId : item.performanceGoalSheetItemId));
				if (params.serialize) {
					$('#delete_item_' + item.performanceGoalSheetItemId).on('click', function (event) {
						genPrevention(event);
						$(this).closest('.performance_item').hide('fast', function () { $(this).remove(); });
					});
				}

				//builds add suggestions button on goal sheet only. May add to review later.
				new_item.find('.performance_addExample_button').css('display', 'block');
				new_item.find('.performance_addExample_button button').attr('id', 'suggest_item_' + params.idPrefix + (typeof item.performanceGoalSheetItemIdTemp !== 'undefined' ? item.performanceFieldId : item.performanceGoalSheetItemId));
				if (params.serialize) {
					$('#suggest_item_' + item.performanceGoalSheetItemId).on('click', function (event) {
						//Pick up HERE: add the ability to bring up modal
						$('.ui.modal.suggest_modal').modal('setting', { autofocus: false }, 10).modal('show');
						$('#suggest_form').find(':input[name=suggested_exampleId]').dropdown('clear');
					});

				}
			}
		}

		// removes delete/add goal from qualitative goals on a submitted goalsheet.
		if (sheet.employeeComplete || params.reviewActive) {
			new_item.find('.performance_delete_button').remove();
			new_item.find('.performance_addExample_button').remove();
			params.parent.find('.add_span').remove();
		}

		// Adds Personal Goal informatin on sheets
		if (typeof item.performanceGoalSheetItem !== 'undefined' && item.performanceGoalSheetItem.performanceGoalSheetItemTexts[0].contents !== '') {
			new_item.find('.performance_description').css('display', 'block');
			new_item.find('.performance_description span').text(item.performanceGoalSheetItem.performanceGoalSheetItemTexts[0].contents);
			//adding result
			if (pfield.name === 'Utilization') {
				var UserQuantitativeResults = { id: sheet.userId, text: sheet.year };
				new_item.find('.performance_result').css('display', 'block');
				new_item.find('.performance_result').addClass('performance_result_' + pfield.name);
				$(function () {
					$.ajax({
						type: "POST",
						url: "/api/Admin/Dapper/UserResults",
						data: JSON.stringify(UserQuantitativeResults),
						dataType: "Json",
						contentType: "application/json",
						success: function (data) {
							var utGoal = "";
							data.forEach(function (d, i, array) { if (d.Name === 'Utilization') { utGoal = d.MetricResult; } });
							if (utGoal !== "") {
								new_item.find('.performance_result span').text(utGoal);
							} else {
								new_item.find('.performance_result span').text('Not available');
							};
						}
					});
				});
			}
		}

		var detail = new_item.find('.item_detail');
		var child = new_item.find('.performance_children');

		//Children Handler
		if (typeof item.performanceReviewSheetItems !== 'undefined') {
			child.css('display', 'block');
			detail.css('display', 'block');
			buildItems({
				items: item.performanceReviewSheetItems,
				parent: child,
				userId: params.userId,
				self: params.self,
				supervisor: params.supervisor,
				lead: params.lead,
				administrator: params.administrator,
				reviewActive: params.reviewActive
			});
		} else if (typeof item.performanceGoalSheetItems !== 'undefined') {
				detail.css('display', 'block');
			if (findField(item.performanceGoalSheetItems[0].performanceFieldId).allowMultiple) {
				child.children('.add_span').css('display', 'block');
				buildItems({
					items: [templateify(item.performanceGoalSheetItems[0])],
					parent: new_item.find('.performance_children'),
					serialize: false,
					userId: params.userId,
					idPrefix: 'template' + item.performanceGoalSheetItems[0].performanceFieldId + '-',
					self: params.self,
					supervisor: params.supervisor,
					lead: params.lead,
					administrator: params.administrator,
					reviewActive: params.reviewActive
					});
					
			} else {
				child.children('span.add_span').remove();
				detail.children('.item_detail').remove();
			}
			child.css('display', 'block');

			buildItems({
				items: item.performanceGoalSheetItems,
				parent: child,
				userId: params.userId,
				serialize: params.serialize,
				idPrefix: params.idPrefix,
				self: params.self,
				supervisor: params.supervisor,
				lead: params.lead,
				administrator: params.administrator,
				reviewActive: params.reviewActive
			});
		} else {
			detail.remove();
		}

		//makes item visible
		if (params.serialize) {
			new_item.css('display', 'block');
			new_item.attr('aria-hidden', false);
		} else {
			if (pfield.allowMultiple && typeof item.performanceReviewSheetId === 'undefined') {
				params.parent.find('.add_span').firstDataChildren(params.parent).css('display', 'block');
				params.parent.find('.add_span a').firstDataChildren(params.parent).text(params.parent.find('.add_span a').first().text() + ' ' + pfield.name);
				params.parent.find('.add_span').firstDataChildren(params.parent).on('click', function (event) {
					genPrevention(event);
					if (params.parent.children('.performance_item[data-serialize="true"]').length < pfield.maximumMultiple) {
						var new_clone = $('#' + id).clone().appendTo(params.parent).show('fast');
						var clone_item_id = temp_id;
						new_clone.attr('id', 'item-' + clone_item_id);
						new_clone.find(':input[name$="IdTemp"]').each(function () {
							if ($(this).not('[name^="parent"]').length > 0 && $(this).parents('.performance_text').length === 0) {
								$(this).val(temp_id);
								temp_id--;
							} else {
								var dParent = $(this).dataParent().dataParent();
								$(this).val(dParent.find(':input[name$="IdTemp"]:not([name^="parent"])').firstDataChildren(dParent).val());
							}
						});
						var parentId = new_clone.dataParent().children(':input[name^="performance"][name$="SheetItemId"]');
						if (parentId.val() !== '') {
							new_clone.children(':input[name^="parent"][name$="SheetItemIdTemp"]').remove();
							$('<input type="hidden" />').appendTo(new_clone).attr('name', 'parent' + parentId.attr('name').charAt(0).toUpperCase() + parentId.attr('name').slice(1)).val(parentId.val());
						}

						// adding suggestion button
						new_clone.find('.performance_addExample_button').firstDataChildren(new_clone).attr('id', 'suggest_item_' + clone_item_id);
						new_clone.find('#suggest_item_' + clone_item_id).first().on('click', function (event) {
							genPrevention(event);
							// replicate the on click to so item
							$('.ui.modal.suggest_modal').modal('setting', { autofocus: false }, 10).modal('show');
							$('#suggest_form').find(':input[name=suggested_exampleId]').dropdown('clear');
							itemId = this.id.split("_");
						});
						//$('body').on('click', ':button.performance_addExample_button', function (event) {itemId = this.id.split("_");});
						new_clone.find('.performance_delete_button').firstDataChildren(new_clone).attr('id', 'delete_item_' + clone_item_id);
						new_clone.find('#delete_item_' + clone_item_id).first().on('click', function (event) {
							genPrevention(event);
							$(this).closest('.performance_item').hide('fast', function () { $(this).remove(); });
						});
						new_clone.find('[id*="' + params.idPrefix + '"]').attr('id', function () {
							return $(this).attr('id').replace(params.idPrefix, temp_id);
						});
						new_clone.find('.performance_comment').remove();
						new_clone.attr('data-serialize', true).find('[data-serialize]:not(.performance_comment)').attr('data-serialize', true);
					} else {
						alert('Maximum number of ' + pfield.name + ' items already reached. An item must be removed before adding another.');
					}

					accordionCleaner();
				});
			} else {
				new_item.css('display', 'block');
				new_item.find('.performance_delete_button').firstDataChildren(new_item).remove();
				new_item.find('.performance_addExample_button').firstDataChildren(new_item).remove();
				new_item.find('.add_span').first().remove();
			}
		}
	});

	accordionCleaner();
}

function templateify(item) {
	var item_temp = JSON.parse(JSON.stringify(item));
	for (var prop in item_temp) {
		if (item_temp.hasOwnProperty(prop) && typeof item_temp[prop] !== 'undefined') {
			if (prop.endsWith('ItemId')) {
				item_temp[prop] = null;
				item_temp[prop + 'Temp'] = -1;
			} else if (prop.endsWith('Texts')) {
				item_temp[prop] = [{
					userId: sheet.userId,
					performanceTextTypeId: performanceTextTypes.find(function (performanceTextType) { return performanceTextType.name === 'Standard'; }).performanceTextTypeId,
					contents: ''
				}];
				if (typeof item_temp.performanceReviewSheetItemId !== 'undefined') {
					item_temp[prop][0]['performanceReviewSheetItemIdTemp'] = -1;
				} else {
					item_temp[prop][0]['performanceGoalSheetItemIdTemp'] = -1;
				}
			} else if (prop.endsWith('Items')) {
				item_temp[prop] = item_temp[prop].filter(function (val, i, array) { return array.findIndex(function (a) { return a.performanceFieldId === val.performanceFieldId; }) === i; });
				item_temp[prop].forEach(function (val, i, array) { array[i] = templateify(val); });
				for (var i = 0; i < item_temp[prop].length; i++) {
					var val = JSON.parse(JSON.stringify(item_temp[prop][i]));
					if (typeof val.performanceFieldId !== 'undefined' && findField(val.performanceFieldId).allowMultiple) {
						for (var j = 1; j < findField(val.performanceFieldId).minimumMultiple; j++) {
							item_temp[prop].splice(i, 0, val);
							i++;
						}
					}
				}
			}
		}
	}
	return item_temp;
}

function saveApprove(sheet, approvalType) {
	if (typeof sheet.performanceReviewSheetItems !== 'undefined') {
		sheet.performanceReviewSheetItems = reserializeSheetItemsRecursive({
			items: $('[data-serialize=true]').filter(function () { return $(this).parents('[data-serialize=true]').length === 0; }),
			itemName: 'performanceReviewSheetItems',
			itemTextName: 'performanceReviewSheetItemTexts'
		});
	} else {
		sheet.performanceGoalSheetItems = reserializeSheetItemsRecursive({
			items: $('[data-serialize=true]').filter(function () { return $(this).parents('[data-serialize=true]').length === 0; }),
			itemName: 'performanceGoalSheetItems',
			itemTextName: 'performanceGoalSheetItemTexts'
		});
	}

	// if we want to save sheet only
	if (typeof approvalType === 'undefined') {
		ajSubmit({
			data: JSON.stringify(sheet),
			url: '/api/Performance/' + (typeof sheet.performanceReviewSheetItems !== 'undefined' ? 'Review' : 'Goal') + 'SheetUpdate',
			workingMessage: 'Updating sheet data...',
			successMessage: 'Sheet updated.',
			failedMessage: 'Failed to update sheet.'
		});
	// else we save and then approve
	} else {
		var approveFunction = function () {
			var data = {};
			data.approved = true;
			data.userId = sheet.userId;
			data.approvalType = approvalType;
			if (typeof sheet.performanceReviewSheetItems !== 'undefined') {
				data.performanceReviewSheetId = sheet.performanceReviewSheetId;
			} else {
				data.performanceGoalSheetId = sheet.performanceGoalSheetId;
			}
			ajSubmit({
				data: JSON.stringify(data),
				url: '/api/Performance/' + (typeof sheet.performanceReviewSheetItems !== 'undefined' ? 'Review' : 'Goal') + 'Sheet/Approve',
				workingMessage: 'Approving sheet status...',
				successMessage: 'Approval complete.',
				successClear: false,
				successFunction: function (data) {
					location.reload();
				},
				successFunctionWait: 3000,
				failedMessage: 'Failed to approve sheet.',
				failedFunction: function (error) {
					console.log(error);
				}
			});
		};

		if (approvalType !== 'Employee' && approvalType !== 'refresh' ) {
			approveFunction();
		} else if (approvalType === "refresh") {
			ajSubmit({
				data: JSON.stringify(sheet),
				url: '/api/Performance/' + (typeof sheet.performanceReviewSheetItems !== 'undefined' ? 'Review' : 'Goal') + 'SheetUpdate',
				workingMessage: 'Reactivating Sheet...',
				successMessage: 'Sheet Reactivated.',
				failedMessage: 'Failed to reactivate sheet. Please save your changes externally and refresh the page.',
				failedClearWait: 6000
			});
		} else {
			ajSubmit({
				data: JSON.stringify(sheet),
				url: '/api/Performance/' + (typeof sheet.performanceReviewSheetItems !== 'undefined' ? 'Review' : 'Goal') + 'SheetUpdate',
				workingMessage: 'Updating sheet data...',
				successMessage: 'Sheet updated.',
				failedMessage: 'Failed to update sheet.',
				successFunction: approveFunction
			});
		}
	}
}

// Testing Function
function fillSheet(text) {
	var comment = typeof text !== 'undefined' ? text : '';
	$(':input[type=radio][name^=performanceScoreId][value=1]').click();
	$('textarea.hide_for_print[name=contents]').val(comment);
}


function createUserTree(data, user) {
	var obj = {
		name: user.name,
		title: user.jobTitle,
		className: '',
		children: []
	};

	if (findDivision(user.divisionId).name === 'Roadway' || findDivision(user.divisionId).name === 'Traffic') {
		obj.className = 'c-engineer';
	}
	else if (findDivision(user.divisionId).name === 'Planning' || findDivision(user.divisionId).name === 'Travel Forecasting') {
		obj.className = 'c-planning';
	}
	else if (findDivision(user.divisionId).name === 'Administration' || findDivision(user.divisionId).name === 'Accounting' || findDivision(user.divisionId).name === 'Marketing' || findDivision(user.divisionId).name === 'Human Resources' || findDivision(user.divisionId).name === 'Information Technology') {
		obj.className = 'c-administration';
	}
	else if (findDivision(user.divisionId).name === 'Data Systems and Analysis') {
		obj.className = 'c-dsa';
	}

	$.each(data, function (i, child) {
		if (child.managerUserId === user.userId) {
			obj.children.push(createUserTree(data, child));
		}
	});
	return obj;
}

//set statuses for the Spot Bonuses
function setBonusStatus(parent, submitted, approved, complete) {
	var overall = 1;
	if (submitted && approved && complete) {
		// if everyone has seen and approved the sheet, the status bar is all blue
		overall = 2;
		parent.find('.status').html('<p>Status: Done');
	} else if ((submitted || approved || complete) === false) {
		// if no one has seen the sheet, the status will be at it's 'default' state in white.
		overall = 0;
		parent.find('.status').html('<p>Status: Needs Attention');
	} else if (overall === 1) {
		parent.find('.status').html(submitted ? '<p>Status: Submission completed.' : approved ? '<p>Status: Approval completed.' : complete ? '<p>Status: Final Approval completed.' : '' + '.</p>');
	}

	parent.find('.sheet_progress_bar .column:nth-child(1) div').addClass(overall <= 1 ? submitted ? 'ready' : 'attention' : overall === 2 ? 'done' : 'default').popup({
		hoverable: true,
		position: 'top center',
		variation: 'wide',
		html: '<p>Submission ' + (submitted ? 'completed' : 'pending') + '.</p>'
	});

	parent.find('.sheet_progress_bar .column:nth-child(2) div').addClass(overall === 1 ? approved ? 'ready' : submitted ? 'attention' : 'default' : overall === 2 ? 'done' : 'default').popup({
		hoverable: true,
		position: 'top center',
		variation: 'wide',
		html: '<p>Approval ' + (approved ? 'completed' : 'pending') + '.</p>'
	});

	parent.find('.sheet_progress_bar .column:nth-child(3) div').addClass(overall === 1 ? complete ? 'ready' : approved ? 'attention' : 'default' : overall === 2 ? 'done' : 'default').popup({
		hoverable: true,
		position: 'top center',
		variation: 'wide',
		html: '<p>Final Approval ' + (complete ? 'completed' : 'pending') + '.</p>'
	});
}

// creates spot Bonus cards.
function createBonusCard(params) {
	params.submitter = typeof params.submitter !== 'undefined' ? params.submitter : '';
	params.name = typeof params.name !== 'undefined' ? params.name : '';
	params.year = typeof params.year !== 'undefined' ? params.year : '';
	var card = '<div class="eight wide column"><div class="ui card employee_card employee_id_' + params.userId + '"><div class="content">';
	card += '<div class="header performance_sheet_card_name">' + params.name + '</div>';
	card += '<div class="meta">';
	card += '<p class="sheet_type">' + ' All Star Award Date: ' + params.year + '</p>';
	card += '<p class="submitteer_name">' + ' Submitted by: ' + params.submitter + '</p>';
	card += '</div></div>';
	card += '<div class="extra content"><div class="ui grid"><div class="row"><div class="eleven wide column">';
	card += '<div class="ui equal width grid ' + params.progressType + '_progress_bar sheet_progress_bar">';
	card += '<div class="column"><div class="' + params.progressType + '_progress" title="viewing status" alt="progress"></div></div>';
	card += '<div class="column"><div class="' + params.progressType + '_progress" title="viewing status" alt="progress"></div></div>';
	card += '<div class="column"><div class="' + params.progressType + '_progress" title="viewing status" alt="progress"></div></div>';
	card += '</div></div><div class="five wide column center aligned">';
	card += '<button class="ui small button options" data-bonusId="' + params.bonusId + '">Details</button>';
	card += '</div></div></div></div></div></div>';
	return $(card);
}

function saveApproveBonus(bonus, approvalType) {

	// if we want to save award only
	if (typeof approvalType === 'undefined') {
		ajSubmit({
			data: JSON.stringify(bonus),
			url: '/api/Rewards/Bonus/Save',
			workingMessage: 'Saving award data...',
			successMessage: 'award saved.',
			failedMessage: 'Failed to save award.'
		});
		// else we save and then approve
	} else {
		var approveFunction = function () {
			var data = {};
			data.approved = true;
			data.approvalType = approvalType;
			data.spotBonusId = bonus.spotBonusId;
			ajSubmit({
				data: JSON.stringify(data),
				url: '/api/Rewards/Bonus/Approve',
				workingMessage: 'Approving award status...',
				successMessage: 'Approval complete.',
				successClear: false,
				successFunction: function (data) {
					location.reload();
				},
				successFunctionWait: 3000,
				failedMessage: 'Failed to approve award.',
				failedFunction: function (error) {
					console.log(error);
				}
			});
		};

		if (approvalType !== 'submitted' && approvalType !== 'refresh') {
			approveFunction();
		} else if (approvalType === "refresh") {
			ajSubmit({
				data: JSON.stringify(bonus),
				url: '/api/Rewards/Bonus/Save',
				workingMessage: 'Reactivating Award...',
				successMessage: 'Award Reactivated.',
				failedMessage: 'Failed to reactivate Award. Please save your changes externally and refresh the page.',
				failedClearWait: 6000
			});
		} else {
			ajSubmit({
				data: JSON.stringify(bonus),
				url: '/api/Rewards/Bonus/Save',
				workingMessage: 'Updating Award data...',
				successMessage: 'Award updated.',
				failedMessage: 'Failed to update Award.',
				successFunction: approveFunction
			});
		}
	}
}

function approveButtonsBonus(approvalType, approvalText, unapprovalType, unapprovalText) {
	if (approvalText !== '') {
		$('.approve_button').text(approvalText).show().on('click', function () {
			var ready = true;
			var bonus = serializeForm('.bonus_form');
			// since sheet is ready, save Award then approve. (leaving this if for future form checks)
			if (ready) {
				if (confirm('Are you sure you want to approve this version of the award? This will move it upwards in the process.')) {
					saveApproveBonus(bonus, approvalType);
				}
			} else {
				alert('Please review your bonus for any errors.');
			}
		});
	} else {
		$('.approve_button').remove();
	}
	if (unapprovalText !== '') {
		$('.unapprove_button').text(unapprovalText).show().on('click', function () {
			if (confirm('Are you sure you want to reset your approval of this award? This will reset the award\'s status and allow you to edit your inputs.')) {
				var data = {};
				var bonus = serializeForm('.bonus_form');
				data.approved = false;
				data.approvalType = 'submitted';
				data.spotBonusId = bonus.spotBonusId;
				ajSubmit({
					data: JSON.stringify(data),
					url: '/api/Rewards/Bonus/Approve', //add the api call
					workingMessage: 'Revoking award approval...',
					successMessage: 'Reset complete.',
					successClear: false,
					successFunction: function (data) {
						location.reload();
					},
					successFunctionWait: 3000,
					failedMessage: 'Failed to revoke award.'
				});
			}
		});
	} else {
		$('.unapprove_button').remove();
	}
}

//set nav buttons for bonus section
function bonusButtons(params) {
	params.self = typeof params.self !== 'undefined' ? params.self : false;
	params.administrator = typeof params.administrator !== 'undefined' ? params.administrator : false;

	params.spotBonusRequestor = typeof params.spotBonusRequestor !== 'undefined' ? params.spotBonusRequestor : false;
	params.spotBonusApprover = typeof params.spotBonusApprover !== 'undefined' ? params.spotBonusApprover : false;
	params.spotBonusCompleter = typeof params.spotBonusCompleter !== 'undefined' ? params.spotBonusCompleter : false;

	params.submitted = typeof params.submitted !== 'undefined' ? params.submitted : false;
	params.approved = typeof params.approved !== 'undefined' ? params.approved : false;
	params.complete = typeof params.complete !== 'undefined' ? params.complete : false;

	$('.sheet_sub_nav, .local_spacer').slideDown();
	$('.expander').show();

	// Remove the entire bar when you are only a viewer of the sheet
	if (!params.self && !params.spotBonusApprover && !params.spotBonusRequestor) {
		approveButtonsBonus('', '', '', '');
	}

	// Filters sheet status versus permissions to show or hide the approval buttons.
	if (params.complete) {
		$('.approve_button').remove();
		$('.unapprove_button').remove();
	} else if (params.approved) {
		if (!params.self && (params.spotBonusCompleter || params.administrator)) {
			approveButtonsBonus('complete', 'Final Approval', 'submitted', 'Send Back to Employee');
		} else {
			if (params.self) {
				approveButtonsBonus('', '', 'submitted', 'Take Back for Edits');
			} else {
				approveButtonsBonus('', '', '', '');
			}
		}
	} else if (params.submitted) {
		if (!params.self && (params.spotBonusApprover || params.administrator)) {
			approveButtonsBonus('approve', 'Approve', 'submitted', 'Send Back to Employee');
		} else {
			if (params.self) {
				approveButtonsBonus('', '', 'submitted', 'Take Back for Edits');
			} else {
				approveButtonsBonus('', '', '', '');
			}
		}
	} else if (params.self) {
		approveButtonsBonus('submitted', 'Send for Approval', '', '');
	} else {
		approveButtonsBonus('', '', '', '');
	}
}

function checkWeights() {
	var total = 0;
	var weights = $('body .performance_item .performance_weight :input[name=weight]').filterSerialized();

	// Reset any weight related warnings
	$('#expected_100').hide();
	$('.performance_weight').removeClass('local_error');
	$('.performance_weight sup.local_error').remove();

	// Find/Set Weights
	weights.each(function () {
		if ($(this).val() === '' || $(this).val() === '0') {
			$(this).parents('.performance_weight').first().addClass('local_error').append('<sub class="tab_error">Please provide a weight for this goal.</sub>');
			total = NaN;
		} else if (!isNaN(total)) {
			total += parseFloat($(this).val());
		}
	});

	if (isNaN(total)) {
		return false;
	} else if (Math.abs(total - 100) < 1) {
		weights.last().val(parseFloat(weights.last().val()) - (total - 100));
		return true;
	} else {
		$('#expected_100').css('display', 'block');
		$('#expected_100 span').text(total.toFixed(3));
		return false;
	}
}

function serializeForm(params) {
	params.items = typeof params.items !== 'undefined' ? params.items : null;
	var obj = {};
	$(params).find(':input:not(button)[data-serialize ="true"]').each(function () {
		if ($(this).filter('input[type="checkbox"]').length > 0) {
			obj[$(this).attr('name')] = $(this).prop('checked');
		}
		else if ($(this).attr('data-primaryKey') === 'true') {
			obj[$(this).attr('name')] = $(this).val() === '-1' ? null : $(this).val();
		}
		else if (typeof $(this).val() === 'string') {
			obj[$(this).attr('name')] = $(this).val().length === 0 ? null : $(this).val();
		}
		else {
			obj[$(this).attr('name')] = $(this).val();
		}
	});
	return obj;
}

function getMonthName(month) {
	switch (month) {
		case 0:
			return "January";
			break;
		case 1:
			return "February";
			break;
		case 2:
			return "March";
			break;
		case 3:
			return "April";
			break;
		case 4:
			return "May";
			break;
		case 5:
			return "June";
			break;
		case 6:
			return "July";
			break;
		case 7:
			return "August";
			break;
		case 8:
			return "September";
			break;
		case 9:
			return "October";
			break;
		case 10:
			return "November";
			break;
		case 11:
			return "December";
			break;
	}
}


