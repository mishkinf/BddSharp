/// <reference path="../../restaurantsexample/scripts/jquery-1.8.2.js" />
describe("Restaurant App Home Page", function () {
    beforeEach(function() {
        // load the html fixture for the home page
        //        jasmine.loadFixture('index.html');
//        jasmine.getFixtures().fixturesPath = '/test/test/fixtures/';
//        jasmine.getFixtures().load('foo-fixtures.html');
    });
    
    xdescribe("When clicking on the Hide All Restaurants button", function() {
        it("should hide all the restaurants ", function () {
            $('#restaurants').show();
            expect($('#restaurants').is('visible')).toBe(true);
            
            $('.hide_restaurants').click();
            expect($('#restaurants').is('visible')).toBe(false);
        }); 
    });

    it("should pass", function() {
        expect(true).toEqual(true);
    });
});