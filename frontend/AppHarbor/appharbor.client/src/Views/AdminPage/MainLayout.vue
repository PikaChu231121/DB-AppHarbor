<template>
    <BanAlert v-if="alertMessage" style="z-index: 1;" :message="alertMessage" @close="alertMessage = ''" />
    <div class="main-layout">
        <div class="sidebar">
            <div @click="toggleSection('appManagement')" class="menu-item section-header">
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M12 22C6.477 22 2 17.523 2 12S6.477 2 12 2s10 4.477 10 10s-4.477 10-10 10M10.5 5.5l7 11m-4-11l-7 11m7-2.5h-7m11 0H16" /></svg>
                &nbsp;&nbsp;&nbsp;应用管理
            </div>
            <div v-show="sections.appManagement" class="section-content">
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '待审核应用' }"
                         @click="selectseleasing();changeSection('appManagement')">
                        <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 2048 2048"><path fill="currentColor" d="M1033 1280q-14 30-21 62t-13 66H384v-128zm-649 384v-128h613q5 33 14 65t23 63zm-128 256h1349l127 128H128V0h1115l549 549v568q-29-26-61-47t-67-37V640h-512V128H256zM1280 219v293h293zm24 805q-109 41-187 128H384v-128zM384 512h640v128H384zm1152 256v128H384V768zm512 1216q0 26-19 45t-45 19t-45-19l-291-290q-39 26-84 39t-92 14q-66 0-124-25t-102-68t-69-102t-25-125t25-124t68-101t102-69t125-26t124 25t101 69t69 102t26 124q0 47-13 92t-40 84l290 291q19 19 19 45m-768-512q0 40 15 75t41 61t61 41t75 15t75-15t61-41t41-61t15-75t-15-75t-41-61t-61-41t-75-15t-75 15t-61 41t-41 61t-15 75" /></svg>&nbsp;&nbsp;待审核
                    </div>
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '已审核应用' }"
                         @click="selectseleased();changeSection('appManagement')">
                        <svg xmlns="http://www.w3.org/2000/svg" width="1.2em" height="1.2em" viewBox="0 0 24 24"><path fill="currentColor" d="m17.275 20.25l3.475-3.45l-1.05-1.05l-2.425 2.375l-.975-.975l-1.05 1.075zM6 9h12V7H6zm12 14q-2.075 0-3.537-1.463T13 18t1.463-3.537T18 13t3.538 1.463T23 18t-1.463 3.538T18 23M3 22V5q0-.825.588-1.412T5 3h14q.825 0 1.413.588T21 5v6.675q-.475-.225-.975-.375T19 11.075V5H5v14.05h6.075q.125.775.388 1.475t.687 1.325L12 22l-1.5-1.5L9 22l-1.5-1.5L6 22l-1.5-1.5zm3-5h5.075q.075-.525.225-1.025t.375-.975H6zm0-4h7.1q.95-.925 2.213-1.463T18 11H6zm-1 6.05V5z" /></svg>&nbsp;&nbsp;已审核
                    </div>
                </div>
            </div>

            <div @click="toggleSection('userManagement')" class="menu-item section-header">
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="currentColor" d="M12.3 12.22A4.92 4.92 0 0 0 14 8.5a5 5 0 0 0-10 0a4.92 4.92 0 0 0 1.7 3.72A8 8 0 0 0 1 19.5a1 1 0 0 0 2 0a6 6 0 0 1 12 0a1 1 0 0 0 2 0a8 8 0 0 0-4.7-7.28M9 11.5a3 3 0 1 1 3-3a3 3 0 0 1-3 3m9.74.32A5 5 0 0 0 15 3.5a1 1 0 0 0 0 2a3 3 0 0 1 3 3a3 3 0 0 1-1.5 2.59a1 1 0 0 0-.5.84a1 1 0 0 0 .45.86l.39.26l.13.07a7 7 0 0 1 4 6.38a1 1 0 0 0 2 0a9 9 0 0 0-4.23-7.68" /></svg>
                &nbsp;&nbsp;&nbsp;用户管理
            </div>
            <div v-show="sections.userManagement" class="section-content">
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '封禁用户' }"
                         @click="searchbanuser();changeSection('userManagement')">
                        <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 1024 1024"><path fill="currentColor" d="M1024 800q0 93-65.5 158.5T800 1024H436l-81-1l-87-2.5l-80-4.5l-75.5-8.5l-57.5-12L13.5 979L0 957q2-88 110-155.5T384 712v-33q-52-23-90-65t-60-98.5t-32-121T192 256q0-65 25-114.5t69-80t101-46T512 0t125 15.5t101 46t69 80T832 256q0 214-71 324q23-4 39-4q93 0 158.5 65.5T1024 800m-64 0q0-50-29-91L709 931q41 29 91 29q66 0 113-47t47-113M768 273q0-103-71.5-156T512 64t-184.5 53T256 273q0 68 10 125t32 106.5t60 82.5t90 46v138q-60 6-117.5 21T232 825.5t-72.5 38T112 898t-16 22q0 14 38 23t114 12t126.5 4t137.5 1q85 0 131-1q-67-66-67-159q0-6 3-28q-1-1-2-1h-1V633q192-41 192-360M640 800q0 50 29 91l222-222q-41-29-91-29q-66 0-113 47t-47 113" /></svg>&nbsp;&nbsp;封禁用户
                    </div>
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '活跃用户' }"
                         @click="searchunbanuser();changeSection('userManagement')">
                        <svg xmlns="http://www.w3.org/2000/svg" width="1.2em" height="1.2em" viewBox="0 0 256 256"><path fill="currentColor" d="M144 157.68a68 68 0 1 0-71.9 0c-20.65 6.76-39.23 19.39-54.17 37.17a8 8 0 0 0 12.25 10.3C50.25 181.19 77.91 168 108 168s57.75 13.19 77.87 37.15a8 8 0 0 0 12.25-10.3c-14.94-17.78-33.52-30.41-54.12-37.17M56 100a52 52 0 1 1 52 52a52.06 52.06 0 0 1-52-52m197.66 33.66l-32 32a8 8 0 0 1-11.32 0l-16-16a8 8 0 0 1 11.32-11.32L216 148.69l26.34-26.35a8 8 0 0 1 11.32 11.32" /></svg>&nbsp;&nbsp;活跃用户
                    </div>
                </div>
            </div>

            <div @click="toggleSection('merchant')" class="menu-item section-header">
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="currentColor" d="M22 7.82a1.25 1.25 0 0 0 0-.19l-2-5A1 1 0 0 0 19 2H5a1 1 0 0 0-.93.63l-2 5a1.25 1.25 0 0 0 0 .19A.58.58 0 0 0 2 8a4 4 0 0 0 2 3.4V21a1 1 0 0 0 1 1h14a1 1 0 0 0 1-1v-9.56A4 4 0 0 0 22 8a.58.58 0 0 0 0-.18M13 20h-2v-4h2Zm5 0h-3v-5a1 1 0 0 0-1-1h-4a1 1 0 0 0-1 1v5H6v-8a4 4 0 0 0 3-1.38a4 4 0 0 0 6 0A4 4 0 0 0 18 12Zm0-10a2 2 0 0 1-2-2a1 1 0 0 0-2 0a2 2 0 0 1-4 0a1 1 0 0 0-2 0a2 2 0 0 1-4 .15L5.68 4h12.64L20 8.15A2 2 0 0 1 18 10" /></svg>
                &nbsp;&nbsp;&nbsp;商家管理
            </div>
            <div v-show="sections.merchant" class="section-content">
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '封禁商家' }"
                         @click="searchbanmerchant();changeSection('MerManagement')">
                        <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M2.997 10.494v4.503c0 2.83 0 4.245.879 5.124c.878.88 2.293.88 5.121.88h3.5m8.5-10.507v2.502m-14 3.994h4m5-.992l3 3.002m0 0l3 3.001m-3-3.001l-3 3.001m3-3.001l3-3.002M17.795 2.001L6.149 2.03c-1.738-.085-2.184 1.187-2.184 1.81c0 .556-.075 1.367-1.14 2.891c-1.066 1.524-.986 1.977-.385 3.032c.498.876 1.766 1.218 2.428 1.276A2.983 2.983 0 0 0 7.99 8.147c1.042 3.045 4.005 3.045 5.325 2.697c1.323-.349 2.456-1.598 2.723-2.697c.156 1.366.63 2.163 2.027 2.711c1.448.568 2.694-.3 3.319-.856s1.026-1.79-.088-3.146c-.768-.936-1.089-1.817-1.194-2.73c-.06-.53-.114-1.099-.506-1.46c-.572-.53-1.393-.69-1.801-.665" color="currentColor" /></svg>
                    &nbsp;&nbsp;封禁商家</div>
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '活跃商家' }"
                         @click="searchunbanMerchant();changeSection('MerManagement')">
                        <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" color="currentColor"><path d="M14.971 7.5s.5 0 1 1c0 0 1.589-2.5 3-3m.025 9.542l.028 4.55A2.503 2.503 0 0 1 16.52 22H5.393a2.503 2.503 0 0 1-2.507-2.499l.086-6.466m6.01-7.022l-3.834-.064a2.005 2.005 0 0 0-1.959 1.389l-1.097 3.416c-.13.404-.139.845.077 1.211c.785 1.334 2.895 3.154 6.248 1.198m-.971-1.83c.39 1.268 1.92 3.493 5.04 2.204" /><path d="M22 7.016a5.01 5.01 0 0 1-5.007 5.016a5.01 5.01 0 0 1-5.007-5.016A5.01 5.01 0 0 1 16.993 2A5.01 5.01 0 0 1 22 7.016" /></g></svg>
                        &nbsp;&nbsp;活跃商家
                    </div>
                </div>
            </div>

            <div @click="toggleSection('comment')" class="menu-item section-header">
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none"><path d="M24 0v24H0V0zM12.593 23.258l-.011.002l-.071.035l-.02.004l-.014-.004l-.071-.035q-.016-.005-.024.005l-.004.01l-.017.428l.005.02l.01.013l.104.074l.015.004l.012-.004l.104-.074l.012-.016l.004-.017l-.017-.427q-.004-.016-.017-.018m.265-.113l-.013.002l-.185.093l-.01.01l-.003.011l.018.43l.005.012l.008.007l.201.093q.019.005.029-.008l.004-.014l-.034-.614q-.005-.019-.02-.022m-.715.002a.02.02 0 0 0-.027.006l-.006.014l-.034.614q.001.018.017.024l.015-.002l.201-.093l.01-.008l.004-.011l.017-.43l-.003-.012l-.01-.01z" /><path fill="currentColor" d="M16 4a3 3 0 0 1 2.995 2.824L19 7v2a3 3 0 0 1 2.995 2.824L22 12v4a3 3 0 0 1-2.824 2.995L19 19v.966c0 1.02-1.143 1.594-1.954 1.033l-.096-.072L14.638 19H11a3 3 0 0 1-1.998-.762l-.14-.134L7 19.5c-.791.593-1.906.075-1.994-.879L5 18.5V17a3 3 0 0 1-2.995-2.824L2 14V7a3 3 0 0 1 2.824-2.995L5 4zm3 7h-8a1 1 0 0 0-1 1v4a1 1 0 0 0 1 1h3.638a2 2 0 0 1 1.28.464l1.088.906A1.5 1.5 0 0 1 18.5 17h.5a1 1 0 0 0 1-1v-4a1 1 0 0 0-1-1m-3-5H5a1 1 0 0 0-1 1v7a1 1 0 0 0 1 1h.5A1.5 1.5 0 0 1 7 16.5v.5l1.01-.757A3 3 0 0 1 8 16v-4a3 3 0 0 1 3-3h6V7a1 1 0 0 0-1-1" /></g></svg>
                &nbsp;&nbsp;&nbsp;评论管理
            </div>
            <div v-show="sections.comment" class="section-content">
                <!-- 评论相关内容 实现 serachComments-->
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '审核评论' }"
                         @click="searchComments();changeSection('commentManagement')">
                        <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="1.5" d="M8 13.5h8m-8-5h4M6.099 19q-1.949-.192-2.927-1.172C2 16.657 2 14.771 2 11v-.5c0-3.771 0-5.657 1.172-6.828S6.229 2.5 10 2.5h4c3.771 0 5.657 0 6.828 1.172S22 6.729 22 10.5v.5c0 3.771 0 5.657-1.172 6.828S17.771 19 14 19c-.56.012-1.007.055-1.445.155c-1.199.276-2.309.89-3.405 1.424c-1.563.762-2.344 1.143-2.834.786c-.938-.698-.021-2.863.184-3.865" color="currentColor" /></svg>
                        &nbsp;&nbsp;已发布评论
                    </div>
                </div>
            </div>


            <div @click="toggleSection('anouncement')" class="menu-item section-header">
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 20 20"><path fill="currentColor" d="M6 8v7h8V8a4 4 0 1 0-8 0m2.03-5.67a2 2 0 1 1 3.95 0A6 6 0 0 1 16 8v6l3 2v1H1v-1l3-2V8a6 6 0 0 1 4.03-5.67M12 18a2 2 0 1 1-4 0z" /></svg>
                &nbsp;&nbsp;&nbsp;通知管理
            </div>
            <div v-show="sections.anouncement" class="section-content">
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '发布公告' }"
                         @click="changeselectedStatus('发布公告'),changeSection('Announcement')">
                        <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="none" stroke="currentColor" stroke-width="2" d="M11 15c3 0 8 4 8 4V3s-5 4-8 4zm-6 0l3 8h4l-3-8m10-1a3 3 0 1 0 0-6m-8 11c1 0 3-1 3-3M2 11c0-3.111 1.791-4 4-4h5v8H6c-2.209 0-4-.889-4-4Z" /></svg>
                        &nbsp;&nbsp;发布通知
                    </div>
                </div>
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '查看公告' }"
                         @click="changeselectedStatus('查看公告'),changeSection('SearchAnnouncement')">
                        <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 48 48"><g fill="none" stroke="currentColor" stroke-linejoin="round" stroke-width="4"><rect width="40" height="26" x="4" y="15" rx="2" /><path stroke-linecap="round" d="m24 7l-8 8h16zM12 24h18m-18 8h8" /></g></svg>
                        &nbsp;&nbsp;查看通知
                    </div>
                </div>
            </div>

            <div @click="toggleSection('report')" class="menu-item section-header">
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"><path d="M8 5H6a2 2 0 0 0-2 2v12a2 2 0 0 0 2 2h5.697M18 14v4h4m-4-7V7a2 2 0 0 0-2-2h-2" /><path d="M8 5a2 2 0 0 1 2-2h2a2 2 0 0 1 2 2v0a2 2 0 0 1-2 2h-2a2 2 0 0 1-2-2m6 13a4 4 0 1 0 8 0a4 4 0 1 0-8 0m-6-7h4m-4 4h3" /></g></svg>
                &nbsp;&nbsp;&nbsp;举报受理
            </div>
            <div v-show="sections.report" class="section-content">
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '已受理' }"
                         @click="changeselectedStatus('已受理'),changeSection('Reported')">
                        <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="currentColor" d="M12 2C7.589 2 4 5.589 4 9.995c-.029 6.445 7.116 11.604 7.42 11.819a.998.998 0 0 0 1.16 0C12.884 21.599 20.029 16.44 20 10c0-4.411-3.589-8-8-8m0 17.735C10.389 18.427 5.979 14.441 6 10c0-3.309 2.691-6 6-6s6 2.691 6 6.005c.021 4.437-4.388 8.423-6 9.73" /><path fill="currentColor" d="M11 11.586L8.707 9.293l-1.414 1.414L11 14.414l5.707-5.707l-1.414-1.414z" /></svg>
                        &nbsp;&nbsp;已受理
                    </div>
                </div>
                <div class="menu">
                    <div class="menu-item"
                         :class="{ active: selectedStatus === '未受理' }"
                         @click="changeselectedStatus('未受理'),changeSection('Reporting')">
                        <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none" stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"><path d="M13 17a4 4 0 1 0 8 0a4 4 0 1 0-8 0" /><path d="M17 13v4h4M12 3v4a1 1 0 0 0 1 1h4" /><path d="M11.5 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h7l5 5v2m0 3v4" /></g></svg>
                        &nbsp;&nbsp; 未受理
                    </div>
                </div>
            </div>
        </div>

        <div class="main-content">
            <!--应用-->
            <div v-if="loading" class="loading">加载中...</div>
            <div v-if="error" class="error">加载失败: {{ error.message }}</div>
            <div v-if="!loading && !error" class="status-display">
                <h2>{{ selectedStatus }}</h2>
            </div>
            <div v-if="section==='appManagement'" class="app-list">
                <div v-for="item in items" :key="item.id" class="app-item">
                    <div class="app-header">
                        <h3>
                            <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 256 256"><path fill="currentColor" d="m64.34 196.07l-9.45 16a8 8 0 1 1-13.78-8.14l9.46-16a8 8 0 1 1 13.77 8.14M232 152h-47.8l-30.73-52a8 8 0 1 0-13.77 8.14l61.41 103.93a8 8 0 0 0 13.78-8.14L193.66 168H232a8 8 0 0 0 0-16m-89.53 0H90.38l68.51-115.93a8 8 0 0 0-13.78-8.14L128 56.89l-17.11-29a8 8 0 1 0-13.78 8.14l21.6 36.55L71.8 152H24a8 8 0 0 0 0 16h118.47a8 8 0 1 0 0-16" /></svg>
                            &nbsp;{{ item.name }}
                        </h3>
                        <span class="app-actions">
                            <button v-if="selectedStatus === '待审核应用'" @click="handleShelve(item)" class="action-button">上架应用</button>
                            <button v-if="selectedStatus === '已审核应用'" @click="handleUnShelve(item)" class="action-button">下架应用</button>
                            <button @click="showDetails(item)" class="action-button">查看应用</button>
                        </span>
                    </div>
                </div>
            </div>

            <!--用户-->
            <div v-if="section==='userManagement' && !userstate" class="app-list">
                <div v-for="user in users" :key="user.id" class="app-item">
                    <div class="user-header">
                        <h3>
                            <svg xmlns="http://www.w3.org/2000/svg" width="1.2em" height="1.2em" viewBox="0 0 256 256"><path fill="currentColor" d="M128 28a100 100 0 1 0 100 100A100.11 100.11 0 0 0 128 28M68.87 198.42a68 68 0 0 1 118.26 0a91.8 91.8 0 0 1-118.26 0m124.3-5.55a75.6 75.6 0 0 0-44.51-34a44 44 0 1 0-41.32 0a75.6 75.6 0 0 0-44.51 34a92 92 0 1 1 130.34 0M128 156a36 36 0 1 1 36-36a36 36 0 0 1-36 36" /></svg>
                            &nbsp;用户昵称：{{ user.nickname }}
                        </h3>
                        <p>用户ID：{{ user.userId }}</p>
                        <p>封禁操作执行管理员ID：{{ user.adminId }}</p>
                        <p>封禁时间：{{ user.time.replace('T', '-') }}</p>
                        <p>封禁原因：{{ user.reason }}</p>
                    </div>
                    <div class="app-actions">
                        <!-- 只保留一个解除封禁按钮 -->
                        <button @click="handleUnban(user)" class="action-button">解除封禁</button>
                    </div>
                </div>
            </div>

            <div v-if="section==='userManagement'&&userstate" class="app-list">
                <div v-for="user in users" :key="user.id" class="app-item">
                    <div class="user-header">
                        <h3>
                            <svg xmlns="http://www.w3.org/2000/svg" width="1.2em" height="1.2em" viewBox="0 0 256 256"><path fill="currentColor" d="M128 28a100 100 0 1 0 100 100A100.11 100.11 0 0 0 128 28M68.87 198.42a68 68 0 0 1 118.26 0a91.8 91.8 0 0 1-118.26 0m124.3-5.55a75.6 75.6 0 0 0-44.51-34a44 44 0 1 0-41.32 0a75.6 75.6 0 0 0-44.51 34a92 92 0 1 1 130.34 0M128 156a36 36 0 1 1 36-36a36 36 0 0 1-36 36" /></svg>
                            &nbsp;用户昵称：{{ user.nickname }}
                        </h3>
                        <p>用户ID：{{ user.id }}</p>
                        <p>用户昵称：{{ user.nickname }}</p>
                        <p>账号注册时间：{{ user.registerTime.replace('T', '-') }}</p>
                    </div>
                    <div class="app-actions">
                        <button @click="handleBan(user)" class="action-button">封禁</button>
                    </div>
                </div>
            </div>

            <!--商家-->
            <div v-if="section==='MerManagement' && !merstate" class="app-list">
                <div v-for="mer in mers" :key="mer.id" class="app-item">
                    <div class="user-header">
                        <h3>
                            <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="currentColor" d="M21 13v7a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1v-7H2v-2l1-5h18l1 5v2zM5 13v6h14v-6zm-.96-2h15.92l-.6-3H4.64zM6 14h8v3H6zM3 3h18v2H3z" /></svg>
                            &nbsp;商家昵称：{{ mer.merchantNickname }}
                        </h3>
                        <p>商家ID：{{ mer.userId}}</p>
                        <p>封禁操作执行管理员ID：{{ mer.adminId }}</p>
                        <p>封禁时间：{{ mer.time.replace('T', '-') }}</p>
                        <p>封禁原因：{{ mer.reason }}</p>
                    </div>
                    <div class="app-actions">
                        <!-- 只保留一个解除封禁按钮 -->
                        <button @click="handleMerUnban(mer)" class="action-button">解除封禁</button>
                    </div>
                </div>
            </div>
            <div v-if="section==='MerManagement'&&merstate" class="app-list">
                <div v-for="mer in mers" :key="mer.id" class="app-item">
                    <div class="user-header">
                        <h3>
                            <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><path fill="currentColor" d="M21 13v7a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1v-7H2v-2l1-5h18l1 5v2zM5 13v6h14v-6zm-.96-2h15.92l-.6-3H4.64zM6 14h8v3H6zM3 3h18v2H3z" /></svg>
                            &nbsp;商家昵称：{{ mer.nickname }}
                        </h3>
                        <p>商家ID：{{ mer.id }}</p>
                        <p>商家昵称：{{ mer.nickname }}</p>
                        <p>商家注册时间：{{ mer.registerTime.replace('T', '-') }}</p>
                    </div>
                    <div class="app-actions">
                        <button @click="handleMerBan(mer)" class="action-button">封禁</button>
                    </div>
                </div>
            </div>

            <!--发布公告-->
            <Announce v-if="section==='Announcement'" />
            <SearchAnnouncement v-if="section==='SearchAnnouncement'" />
            <!--举报受理-->
            <Reported v-if="section==='Reported'" />
            <Reporting v-if="section==='Reporting'" />

            <!-- 封禁成功弹窗 -->
            <div v-if="showBanSuccessPopup" class="popup-overlay" @click="closeBanSuccessPopup">
                <div class="popup-content success-popup" @click.stop>
                    <h3>
                        <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 36 36"><path fill="currentColor" d="M18 2a16 16 0 1 0 16 16A16 16 0 0 0 18 2m0 30a14 14 0 1 1 14-14a14 14 0 0 1-14 14" class="clr-i-outline clr-i-outline-path-1" /><path fill="currentColor" d="M28 12.1a1 1 0 0 0-1.41 0l-11.1 11.05l-6-6A1 1 0 0 0 8 18.53L15.49 26L28 13.52a1 1 0 0 0 0-1.42" class="clr-i-outline clr-i-outline-path-2" /><path fill="none" d="M0 0h36v36H0z" /></svg>
                        成功执行
                    </h3>
                    <p>管理员权限操作成功！</p>
                    <button @click="closeBanSuccessPopup" class="popup-close-button">关闭</button>
                </div>
            </div>

            <!-- 封禁解除成功弹窗 -->
            <div v-if="showUnBanSuccessPopup" class="popup-overlay" @click="closeBanSuccessPopup">
                <div class="popup-content success-popup" @click.stop>
                    <h3>
                        <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 36 36"><path fill="currentColor" d="M18 2a16 16 0 1 0 16 16A16 16 0 0 0 18 2m0 30a14 14 0 1 1 14-14a14 14 0 0 1-14 14" class="clr-i-outline clr-i-outline-path-1" /><path fill="currentColor" d="M28 12.1a1 1 0 0 0-1.41 0l-11.1 11.05l-6-6A1 1 0 0 0 8 18.53L15.49 26L28 13.52a1 1 0 0 0 0-1.42" class="clr-i-outline clr-i-outline-path-2" /><path fill="none" d="M0 0h36v36H0z" /></svg>
                        成功执行
                    </h3>
                    <p>管理员权限操作成功！</p>
                    <button @click="closeBanSuccessPopup" class="popup-close-button">关闭</button>
                </div>
            </div>

            <!--评论 实现 handleDeleteComment -->
            <div v-if="section==='commentManagement'" class="app-list">
                <div v-for="comment in comments" :key="comment.id" class="app-item">
                    <div class="user-header">
                        <h3>用户: {{ comment.nickname }}</h3>
                        <p>评论应用: {{ comment.appName }}</p>
                        <p>评分: {{ comment.score }}</p>
                        <p>评论时间: {{ comment.publishTime.replace('T', '-') }}</p>
                    </div>
                    <div class="comment-content">
                        <p>{{ comment.content }}</p>
                    </div>
                    <div class="app-actions">
                        <button @click="handleDeleteComment(comment)" class="action-button">封禁</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- 弹窗 -->
    <div v-if="showPopup" class="popup-overlay" @click="closePopup">
        <div class="popup-content" @click.stop>
            <h3>
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 256 256"><path fill="currentColor" d="m64.34 196.07l-9.45 16a8 8 0 1 1-13.78-8.14l9.46-16a8 8 0 1 1 13.77 8.14M232 152h-47.8l-30.73-52a8 8 0 1 0-13.77 8.14l61.41 103.93a8 8 0 0 0 13.78-8.14L193.66 168H232a8 8 0 0 0 0-16m-89.53 0H90.38l68.51-115.93a8 8 0 0 0-13.78-8.14L128 56.89l-17.11-29a8 8 0 1 0-13.78 8.14l21.6 36.55L71.8 152H24a8 8 0 0 0 0 16h118.47a8 8 0 1 0 0-16" /></svg>
                &nbsp;&nbsp;&nbsp;{{ selectedApp.name }}
            </h3>
            <p>&nbsp;&nbsp;应用版本: &nbsp;&nbsp;&nbsp;{{ selectedApp.version }}</p>
            <p>&nbsp;&nbsp;应用发行商: &nbsp;&nbsp;&nbsp;{{ selectedApp.merchantNickname }}</p>
            <p>&nbsp;&nbsp;应用类型:&nbsp;&nbsp;&nbsp; {{ selectedApp.category }}</p>
            <p>&nbsp;&nbsp;应用ID: &nbsp;&nbsp;&nbsp;{{ selectedApp.id }}</p>
            <p>&nbsp;&nbsp;应用价格:&nbsp;&nbsp;&nbsp; {{ selectedApp.price === 0 ? '免费' : selectedApp.price }}</p>
            <p>
                &nbsp;&nbsp;应用发布折扣: &nbsp;&nbsp;&nbsp;
                {{ selectedApp.discount === 1 ? '原价' : (selectedApp.discount * 10).toFixed(1) + '折' }}
            </p>

            <p>&nbsp;&nbsp;应用描述: &nbsp;&nbsp;&nbsp;{{ selectedApp.description }}</p>
            <button @click="closePopup" class="popup-close-button">关闭</button>
        </div>
    </div>


    <!-- 确认弹窗 -->
    <div v-if="showConfirmPopup" class="popup-overlay" @click="cancelShelve">
        <div class="popup-content confirm-popup" @click.stop>
            <h3>
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none"><circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="1.5" /><path stroke="currentColor" stroke-linecap="round" stroke-width="1.5" d="M12 17v-6" /><circle cx="1" cy="1" r="1" fill="currentColor" transform="matrix(1 0 0 -1 11 9)" /></g></svg>
                确认上架
            </h3>
            <p>您确定要上架 {{ appToShelve ? appToShelve.name : '' }} 应用吗？</p>
            <div class="confirm-buttons">
                <button @click="confirmShelve" class="popup-confirm-button">确定</button>
                <button @click="cancelShelve" class="popup-cancel-button">取消</button>
            </div>
        </div>
    </div>

    <!-- 确认下架弹窗 -->
    <div v-if="showConfirmUnPopup" class="popup-overlay" @click="cancelShelve">
        <div class="popup-content confirm-popup" @click.stop>
            <h3>
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none"><circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="1.5" /><path stroke="currentColor" stroke-linecap="round" stroke-width="1.5" d="M12 17v-6" /><circle cx="1" cy="1" r="1" fill="currentColor" transform="matrix(1 0 0 -1 11 9)" /></g></svg>
                确认下架
            </h3>
            <p>您确定要下架 {{ appToUnShelve ? appToUnShelve.name : '' }} 应用吗？</p>
            <div class="confirm-buttons">
                <button @click="confirmUnShelve" class="popup-confirm-button">确定</button>
                <button @click="cancelUnShelve" class="popup-cancel-button">取消</button>
            </div>
        </div>
    </div>
    <div v-if="showSuccessPopup" class="popup-overlay" @click="closeSuccessPopup">
        <div class="popup-content success-popup" @click.stop>
            <h3>
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 36 36"><path fill="currentColor" d="M18 2a16 16 0 1 0 16 16A16 16 0 0 0 18 2m0 30a14 14 0 1 1 14-14a14 14 0 0 1-14 14" class="clr-i-outline clr-i-outline-path-1" /><path fill="currentColor" d="M28 12.1a1 1 0 0 0-1.41 0l-11.1 11.05l-6-6A1 1 0 0 0 8 18.53L15.49 26L28 13.52a1 1 0 0 0 0-1.42" class="clr-i-outline clr-i-outline-path-2" /><path fill="none" d="M0 0h36v36H0z" /></svg>
                管理员审核成功
            </h3>
            <p>该应用已成功上架！</p>
            <button @click="closeSuccessPopup" class="popup-close-button">关闭</button>
        </div>
    </div>
    <div v-if="showSuccessUnPopup" class="popup-overlay" @click="closeSuccessUnPopup">
        <div class="popup-content success-popup" @click.stop>
            <h3>
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 36 36"><path fill="currentColor" d="M18 2a16 16 0 1 0 16 16A16 16 0 0 0 18 2m0 30a14 14 0 1 1 14-14a14 14 0 0 1-14 14" class="clr-i-outline clr-i-outline-path-1" /><path fill="currentColor" d="M28 12.1a1 1 0 0 0-1.41 0l-11.1 11.05l-6-6A1 1 0 0 0 8 18.53L15.49 26L28 13.52a1 1 0 0 0 0-1.42" class="clr-i-outline clr-i-outline-path-2" /><path fill="none" d="M0 0h36v36H0z" /></svg>
                管理员下架成功
            </h3>
            <p>该应用已成功下架！</p>
            <button @click="closeSuccessUnPopup" class="popup-close-button">关闭</button>
        </div>
    </div>
    <!-- 封禁确认弹窗 -->
    <div v-if="showBanConfirmPopup" class="popup-overlay" @click="cancelBan">
        <div class="popup-content ban-confirm-popup" @click.stop>
            <h3>
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none"><circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="1.5" /><path stroke="currentColor" stroke-linecap="round" stroke-width="1.5" d="M12 17v-6" /><circle cx="1" cy="1" r="1" fill="currentColor" transform="matrix(1 0 0 -1 11 9)" /></g></svg>
                确认封禁&nbsp;{{ userToBan ? userToBan.nickname : '' }}
            </h3>
            <p>请填写封禁理由：</p>
            <textarea v-model="banReason" rows="4" placeholder="请输入封禁理由"></textarea>
            <div class="confirm-buttons">
                <button @click="confirmBan" class="popup-confirm-button">确定封禁</button>
                <button @click="cancelBan" class="popup-cancel-button">取消</button>
            </div>
        </div>
    </div>

    <!-- 封禁解除确认弹窗 -->
    <div v-if="showUnbanConfirmPopup" class="popup-overlay" @click="cancelUnban">
        <div class="popup-content unban-confirm-popup" @click.stop>
            <h3>
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none"><circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="1.5" /><path stroke="currentColor" stroke-linecap="round" stroke-width="1.5" d="M12 17v-6" /><circle cx="1" cy="1" r="1" fill="currentColor" transform="matrix(1 0 0 -1 11 9)" /></g></svg>
                确认解除封禁&nbsp;{{ userToUnban ? userToUnban.nickname : '' }}
            </h3>
            <p>您确定要解除用户 {{ userToUnban ? userToUnban.nickname : '' }} 的封禁吗？</p>
            <div class="confirm-buttons">
                <button @click="confirmUnban" class="popup-confirm-button">确定</button>
                <button @click="cancelUnban" class="popup-cancel-button">取消</button>
            </div>
        </div>
    </div>

    <!-- 封禁商家确认弹窗 -->
    <div v-if="showMerBanConfirmPopup" class="popup-overlay" @click="cancelMerBan">
        <div class="popup-content ban-confirm-popup" @click.stop>
            <h3>
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none"><circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="1.5" /><path stroke="currentColor" stroke-linecap="round" stroke-width="1.5" d="M12 17v-6" /><circle cx="1" cy="1" r="1" fill="currentColor" transform="matrix(1 0 0 -1 11 9)" /></g></svg>
                确认封禁商家&nbsp;{{ merToBan ? merToBan.nickname : '' }}
            </h3>
            <p>请填写封禁理由：</p>
            <textarea v-model="merBanReason" rows="4" placeholder="请输入封禁理由"></textarea>
            <div class="confirm-buttons">
                <button @click="confirmMerBan" class="popup-confirm-button">确定封禁</button>
                <button @click="cancelMerBan" class="popup-cancel-button">取消</button>
            </div>
        </div>
    </div>

    <!-- 封禁解除确认弹窗 -->
    <div v-if="showMerUnbanConfirmPopup" class="popup-overlay" @click="cancelMerUnban">
        <div class="popup-content unban-confirm-popup" @click.stop>
            <h3>
                <svg xmlns="http://www.w3.org/2000/svg" width="1em" height="1em" viewBox="0 0 24 24"><g fill="none"><circle cx="12" cy="12" r="10" stroke="currentColor" stroke-width="1.5" /><path stroke="currentColor" stroke-linecap="round" stroke-width="1.5" d="M12 17v-6" /><circle cx="1" cy="1" r="1" fill="currentColor" transform="matrix(1 0 0 -1 11 9)" /></g></svg>
                确认解除封禁&nbsp;{{ merToUnban ? merToUnban.merchantNickname : '' }}
            </h3>
            <p>您确定要解除商家 {{ merToUnban ? merToUnban.merchantNickname : '' }} 的封禁吗？</p>
            <div class="confirm-buttons">
                <button @click="confirmMerUnban" class="popup-confirm-button">确定</button>
                <button @click="cancelMerUnban" class="popup-cancel-button">取消</button>
            </div>
        </div>
    </div>

    <!-- 封禁评论确认弹窗 -->
    <div v-if="showCommentBanConfirmPopup" class="popup-overlay" @click="cancelCommentBan">
        <div class="popup-content ban-confirm-popup" @click.stop>
            <h3>确认封禁这条评论吗？</h3>
            <p>请填写封禁理由：</p>
            <textarea v-model="banReason" rows="4" placeholder="请输入封禁理由"></textarea>
            <div class="confirm-buttons">
                <button @click="confirmCommentBan" class="popup-confirm-button">确定封禁</button>
                <button @click="cancelCommentBan" class="popup-cancel-button">取消</button>
            </div>
        </div>
    </div>
</template>

<script>
    import axios from 'axios';
    import Cookies from 'js-cookie';
    import BanAlert from './BanAlert.vue';
    import Announce from './Announce.vue';
    import SearchAnnouncement from './SearchAnnouncement.vue';
    import Reported from './Reported.vue';
    import Reporting from './Reporting.vue';

    export default {
        name: "MainLayout",
        components: {
            BanAlert,
            Announce,
            SearchAnnouncement,
            Reported,
            Reporting,
        },
        data() {
            return {
                alertMessage: '',

                items: [],
                users: [],
                mers: [],
                comments:[],
                loading: false,
                error: null,
                userstate: 0,
                merstate: 0,
                section: null,
                selectedStatus: '请在左侧选择你要执行的管理权限',
                sections: {
                    appManagement: false,
                    userManagement: false,
                    comment: false,
                    merchant: false,
                    announcement: false,
                    report:false,
                },
                showPopup: false,
                showConfirmPopup: false,
                showConfirmUnPopup: false,
                showSuccessPopup: false,
                showSuccessUnPopup: false,
                showBanConfirmPopup: false,
                showBanSuccessPopup: false,
                showUnbanConfirmPopup: false,
                showMerBanConfirmPopup: false,
                selectedApp: null,
                appToShelve: null,
                appToUnShelve: null,
                selectedUser: null,
                merToBan: null,
                merBanReason: '',

                userToUnban: null,
                showUnbanConfirmPopup: false,
                showMerUnbanConfirmPopup: false,
                merToUnban: null,

                commentToBan: null,
                showCommentBanConfirmPopup: false,
            };
        },
        methods: {
            handleMerUnban(merchant) {
                this.merToUnban = merchant;
                this.showMerUnbanConfirmPopup = true;
            },
            confirmMerUnban() {
                const token = Cookies.get('token');
                if (!token) {
                    alert('未提供 token');
                    return;
                }

                const formData = new FormData();
                formData.append('mytoken', token);
                formData.append('merchant_id', this.merToUnban.userId);

                axios.post(`${this.$Url}/api/BanMerchant/unbanmerchant`, formData)
                    .then(response => {
                        this.showMerUnbanConfirmPopup = false;
                        this.merToUnban = null;
                        this.showBanSuccessPopup = true;
                        this.searchbanmerchant();
                    })
                    .catch(error => {
                        console.error('解除封禁商家失败:', error);
                        alert('解除封禁商家失败，请重试');
                    });
            },
            cancelMerUnban() {
                this.showMerUnbanConfirmPopup = false;
                this.merToUnban = null;
            },
            handleMerBan(merchant) {
                this.merToBan = merchant;
                this.showMerBanConfirmPopup = true;
            },
            confirmMerBan() {
                const token = Cookies.get('token');
                if (!token) {
                    alert('未提供 token');
                    return;
                }

                const formData = new FormData();
                formData.append('mytoken', token);
                formData.append('merchant_id', this.merToBan.id);
                formData.append('reason', this.merBanReason);

                axios.post(`${this.$Url}/api/BanMerchant/banmerchant`, formData)
                    .then(response => {
                        this.showMerBanConfirmPopup = false;
                        this.merBanReason = '';
                        this.merToBan = null;
                        this.showBanSuccessPopup = true;
                        this.searchbanmerchant();
                    })
                    .catch(error => {
                        console.error('封禁商家失败:', error);
                        alert('封禁商家失败，请重试');
                    });
            },
            cancelMerBan() {
                this.showMerBanConfirmPopup = false;
                this.merBanReason = '';
                this.merToBan = null;
            },
            handleUnban(user) {
                this.userToUnban = user;
                this.showUnbanConfirmPopup = true;
            },
            confirmUnban() {
                const token = Cookies.get('token');
                if (!token) {
                    alert('未提供 token');
                    return;
                }

                const formData = new FormData();
                formData.append('mytoken', token);
                formData.append('user_id', this.userToUnban.userId); // 确保传递正确的用户 ID

                axios.post(`${this.$Url}/api/BanUser/unbanuser`, formData)
                    .then(response => {
                        this.showUnbanConfirmPopup = false;
                        this.userToUnban = null;
                        this.showBanSuccessPopup = true;
                        this.searchbanuser();
                    })
                    .catch(error => {
                        console.error('解除封禁失败:', error);
                        alert('解除封禁失败，请重试');
                    });
            },
            cancelUnban() {
                this.showUnbanConfirmPopup = false;
                this.userToUnban = null;
            },
            handleBan(user) {
                this.userToBan = user;
                this.showBanConfirmPopup = true;
            },
            confirmBan() {
                const token = Cookies.get('token');
                if (!token) {
                    alert('未提供 token');
                    return;
                }

                const formData = new FormData();
                formData.append('mytoken', token);
                formData.append('user_id', this.userToBan.id);
                formData.append('reason', this.banReason);

                axios.post(`${this.$Url}/api/BanUser/banuser`, formData)
                    .then(response => {
                        this.showBanConfirmPopup = false;
                        this.banReason = '';
                        this.userToBan = null;
                        this.showBanSuccessPopup = true;
                        this.searchbanuser();
                    })
                    .catch(error => {
                        console.error('封禁失败:', error);
                        //this.alertMessage = '请输入封禁理由';
                        alert('请输入封禁理由');
                    });
            },
            cancelBan() {
                this.showBanConfirmPopup = false;
                this.banReason = '';
                this.userToBan = null;
            },
            closeBanSuccessPopup() {
                this.showBanSuccessPopup = false;
            },
            closeUserPopup() {
                this.showUserPopup = false;
                this.selectedUser = null;
            },
            toggleSection(section) {
                this.section = section;
                this.sections[section] = !this.sections[section];
            },
            changeSection(section) {
                this.section = section;
            },
            selectseleasing() {
                this.selectedStatus = '待审核应用';
                this.fetchData(`${this.$Url}/api/application/selectseleasing`);
            },
            selectseleased() {
                this.selectedStatus = '已审核应用';
                const token = Cookies.get('token');
                const formData = new FormData();
                formData.append('token', token);
                this.fetchData(`${this.$Url}/api/application/selectseleased`, formData);
            },
            searchbanuser() {
                this.selectedStatus = '封禁用户';
                this.userstate = 0;
                axios.post(`${this.$Url}/api/banuser/searchbanuser`)
                    .then(response => {
                        this.users = response.data;
                    })
                    .catch(error => {
                        this.error = error;
                    })
                    .finally(() => {
                        this.loading = false;
                    });
            },
            searchunbanuser() {
                this.selectedStatus = '活跃用户';
                this.userstate = 1;
                axios.post(`${this.$Url}/api/user/searchunbanuser`)
                    .then(response => {
                        this.users = response.data.$values;
                    })
                    .catch(error => {
                        this.error = error;
                    })
                    .finally(() => {
                        this.loading = false;
                    });
            },
            searchbanmerchant() {
                this.selectedStatus = '封禁商家';
                this.merstate = 0;
                axios.post(`${this.$Url}/api/banmerchant/searchbanmerchant`)
                    .then(response => {
                        this.mers = response.data;
                    })
                    .catch(error => {
                        this.error = error;
                    })
                    .finally(() => {
                        this.loading = false;
                    });
            },
            searchunbanMerchant() {
                this.selectedStatus = '活跃商家';
                this.merstate = 1;
                axios.post(`${this.$Url}/api/merchant/searchunbanmerchant`)
                    .then(response => {
                        this.mers = response.data.$values;
                        console.log(this.mers);
                    })
                    .catch(error => {
                        this.error = error;
                    })
                    .finally(() => {
                        this.loading = false;
                    });
            },
            searchComments() {
                this.selectedStatus = '审核评论';
                axios.get(`${this.$Url}/api/comment/getallcomments`)
                    .then(response => {
                        this.comments = response.data.$values;
                        /*console.log(this.mers);*/
                    })
                    .catch(error => {
                        this.error = error;
                    })
                    .finally(() => {
                        this.loading = false;
                    });
            },
            handleDeleteComment(comment) {
                this.commentToBan = comment;
                this.showCommentBanConfirmPopup = true;
            },
            confirmCommentBan() {
                const token = Cookies.get('token');
                if (!token) {
                    alert('未提供 token');
                    return;
                }

                const formData = new FormData();
                formData.append('mytoken', token);
                formData.append('comment_id', this.commentToBan.id);
                formData.append('reason', this.banReason);

                axios.post(`${this.$Url}/api/bancomment`, formData)
                    .then(response => {
                        this.showCommentBanConfirmPopup = false;
                        this.banReason = '';
                        this.commentToBan = null;
                        this.showBanSuccessPopup = true;
                        this.searchComments();
                    })
                    .catch(error => {
                        console.error('封禁失败:', error);
                        alert('请输入封禁理由');
                    });
            },
            cancelCommentBan() {
                this.showCommentBanConfirmPopup = false;
                this.banReason = '';
                this.commentToBan = null;
            },
            fetchData(url, data = null) {
                this.loading = true;
                this.error = null;
                axios.post(url, data)
                    .then(response => {
                        this.items = response.data.$values;
                    })
                    .catch(error => {
                        this.error = error;
                    })
                    .finally(() => {
                        this.loading = false;
                    });
            },
            handleShelve(item) {
                this.appToShelve = item; // 保存要上架的应用信息
                this.showConfirmPopup = true; // 显示确认弹窗
            },
            handleUnShelve(item) {
                this.appToUnShelve = item; // 保存要上架的应用信息
                this.showConfirmUnPopup = true; // 显示确认弹窗
            },
            confirmShelve() {
                const token = Cookies.get('token');
                if (!token) {
                    alert('未提供 token');
                    return;
                }

                const formData = new FormData();
                formData.append('Id', this.appToShelve.id);
                formData.append('token', token);
                axios.post(`${this.$Url}/api/application/confirmrelease`, formData)
                    .then(response => {
                        this.showConfirmPopup = false; // 隐藏确认弹窗
                        this.showSuccessPopup = true; // 显示成功弹窗
                        this.fetchData(`${this.$Url}/api/application/selectseleasing`);
                    })
                    .catch(error => {
                        console.error('审核失败:', error);
                        alert('审核失败，请重试');
                    })
                    .finally(() => {
                        this.appToShelve = null; // 清除应用信息
                    });
            },
            confirmUnShelve() {
    const token = Cookies.get('token');
    if (!token) {
        alert('未提供 token');
        return;
    }

    const formData = new FormData();
    formData.append('Id', this.appToUnShelve.id);
    formData.append('token', token);
    axios.post(`${this.$Url}/api/application/confirmdown`, formData)
        .then(response => {
            this.showConfirmUnPopup = false; // 隐藏确认弹窗
            this.showSuccessUnPopup = true; // 显示成功弹窗
            this.fetchData(`${this.$Url}/api/application/selectseleased`);
        })
        .catch(error => {
            console.error('下架失败:', error);
            alert('下架失败，请重试');
        })
        .finally(() => {
            this.appToUnShelve = null; // 清除应用信息
        });
},
            cancelShelve() {
                this.showConfirmPopup = false; // 取消上架操作，隐藏确认弹窗
                this.appToUnShelve = null; // 清除应用信息
            },
            cancelUnShelve() {
                this.showConfirmUnPopup = false; // 取消上架操作，隐藏确认弹窗
                this.appToUnShelve = null; // 清除应用信息
            },
            showDetails(item) {
                this.selectedApp = item;
                this.showPopup = true;
            },
            closePopup() {
                this.showPopup = false;
                this.selectedApp = null;
            },
            closeSuccessPopup() {
                this.showSuccessPopup = false;
                this.appToShelve = null;
            },
            closeSuccessUnPopup() {
                this.showSuccessUnPopup = false;
                this.appToUnShelve = null;
            },
            changeselectedStatus(state) {
                this.selectedStatus = state;
            }
        }
    };
</script>

<style scoped>
    @import './MainLayout.css';
</style>