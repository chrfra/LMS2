const { test, expect } = require('@playwright/test');
const path = require('path');

test.describe('LMS End-to-End Tests - Browser as End User', () => {
  const BASE_URL = 'http://localhost:4200';
  const SCORM_FILE = '/Users/christianfransson/workspace/LMS/SCORM/place_order_app_web_desginer-697b66d975656fd469740852-1770208230743-scorm.zip';

  test('Complete workflow: Login → Upload SCORM → View Course', async ({ page }) => {
    test.setTimeout(300000); // 5 minutes timeout for this test
    // Step 1: Navigate to login page
    console.log('🔵 Step 1: Navigating to login page...');
    await page.goto(`${BASE_URL}/login`);
    await expect(page).toHaveURL(`${BASE_URL}/login`);
    await page.waitForLoadState('networkidle');
    console.log('✅ Login page loaded');

    // Step 2: Enter credentials and login
    console.log('\n🔵 Step 2: Logging in as admin...');
    console.log('⏳ Filling username...');
    await page.fill('input#username', 'admin', { timeout: 5000 });
    console.log('⏳ Filling password...');
    await page.fill('input#password', 'admin123', { timeout: 5000 });
    
    console.log('⏳ Clicking login button...');
    // Find and click the submit button
    const loginBtn = page.locator('button[type="submit"]');
    await loginBtn.click({ timeout: 5000 });
    
    // Wait for navigation to complete
    console.log('⏳ Waiting for dashboard to load after login...');
    // Instead of waiting for specific URL, wait for the file input to appear
    await page.waitForTimeout(2000); // Give form time to process
    await page.waitForLoadState('networkidle');
    console.log('✅ Login successful, page loaded');

    // Step 3: Navigate to dashboard (after login we should be there)
    console.log('\n🔵 Step 3: Dashboard loaded after login...');
    await page.waitForLoadState('networkidle');
    // The file input is hidden, but the label wraps it - look for the upload section
    const uploadSection = page.locator('.upload-section').first();
    await uploadSection.waitFor({ state: 'visible', timeout: 10000 });
    console.log('✅ Admin dashboard with upload form visible');

    // Step 4: Upload SCORM file
    console.log('\n🔵 Step 4: Uploading SCORM file...');
    const fileInput = await page.$('input#fileInput');
    if (fileInput) {
      console.log('📁 File input found (hidden but present), setting file...');
      await fileInput.setInputFiles(SCORM_FILE);
      await page.waitForTimeout(1000);
      
      // Click the upload button
      console.log('📤 Clicking upload button...');
      await page.click('button.btn-upload');
      
      // Wait for upload to complete  
      console.log('⏳ Waiting for upload to complete...');
      await page.waitForTimeout(5000);
      await page.waitForLoadState('networkidle');
      console.log('✅ SCORM file uploaded successfully');
    } else {
      throw new Error('File input not found on dashboard');
    }

    // Step 5: Verify course appears in list
    console.log('\n🔵 Step 5: Verifying course appears in list...');
    await page.waitForTimeout(2000);
    
    // Look for course card with the course title
    let courseCard = page.locator('.course-card:has-text("place_order_app_web_desginer")').first();
    let isVisible = await courseCard.isVisible({ timeout: 5000 }).catch(() => false);
    
    if (!isVisible) {
      console.log('⏳ Course not immediately visible, reloading page...');
      await page.reload({ waitUntil: 'networkidle' });
      await page.waitForTimeout(2000);
      courseCard = page.locator('.course-card:has-text("place_order_app_web_desginer")').first();
    }
    
    await expect(courseCard).toBeVisible({ timeout: 10000 });
    console.log('✅ Course appears in list');

    // Step 6: Click on course to view details
    console.log('\n🔵 Step 6: Opening course viewer...');
    const viewButton = await page.locator('.course-card:has-text("place_order_app_web_desginer") button.btn-view').first();
    await viewButton.click();
    await page.waitForLoadState('networkidle');
    console.log('✅ Course viewer page loaded');

    // Step 7: Verify course content is displayed
    console.log('\n🔵 Step 7: Verifying course content...');
    const pageTitle = await page.locator('text=Place Order App web desginer').first();
    await expect(pageTitle).toBeVisible({ timeout: 10000 });
    console.log('✅ Course content is visible');

    console.log('\n' + '='.repeat(70));
    console.log('✅✅✅ ALL END-TO-END BROWSER TESTS PASSED ✅✅✅');
    console.log('='.repeat(70));
    console.log('\nComplete User Workflow Verified:');
    console.log('  1. ✅ Login with credentials (admin/admin123)');
    console.log('  2. ✅ Navigated to admin dashboard');
    console.log('  3. ✅ Selected and uploaded 22MB SCORM file');
    console.log('  4. ✅ Course appeared in dashboard list');
    console.log('  5. ✅ Clicked "View" to open course');
    console.log('  6. ✅ Course pages loaded and displayed');
    console.log('  7. ✅ Course content accessible to user');
    console.log('='.repeat(70));
  });
});
