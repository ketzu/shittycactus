mergeInto(LibraryManager.library, {
  InitKongAPI: function () {
	if(kongregateAPI !== undefined){
		kongregateAPI.loadAPI(function(){
			window.kongregate = kongregateAPI.getAPI();
		});
	}else{
		console.log("Kongapi is undefined.");
	}
  },

  SubmitHeight: function (height) {
	console.log("Submitting height!");
	if(window.kongregate !== undefined) {
		window.kongregate.stats.submit('Height', height);	
	}else{
	console.log("Kongapi is not set.");
	}
  },

  SubmitCoins: function (coins) {
	console.log("Submitting coins!");
	if(window.kongregate !== undefined) {
		window.kongregate.stats.submit('Height', coins);
	}else{
	console.log("Kongapi is not set.");
	}
  }
});