"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const functions = require("firebase-functions");
const admin = require("firebase-admin");
if (admin.apps.length === 0) {
    // Firebase 앱이 이미 초기화되어 있지 않은지 확인
    admin.initializeApp();
}
exports.scheduledFunctionCrontab = functions.pubsub
    .schedule("0 0,6,12,18 * * *")
    .timeZone("Asia/Seoul") // 이 부분을 해당 지역의 시간대로 변경하세요.
    .onRun(async (context) => {
    console.log("This will be run every day at 6am, 12pm, 6pm, and 12am Seoul time!");
    // 여기에 랭킹을 업데이트하는 로직을 구현합니다.
    const recordsRef = admin.database().ref("records");
    const maxJumpRef = admin.database().ref("ranking/max_jump");
    const addicterRef = admin.database().ref("ranking/addicter");
    const snapshots = await recordsRef.once("value");
    const records = snapshots.val();
    // count_jump 값을 기준으로 정렬, 상위 10명 추출
    const topJumpers = Object.keys(records)
        .map((userId) => {
        const countJump = records[userId].map1 ?
            records[userId].map1.count_jump :
            0;
        return { userId, countJump };
    })
        .sort((a, b) => b.countJump - a.countJump)
        .slice(0, 10)
        .reduce((acc, current) => (Object.assign(Object.assign({}, acc), { [current.userId]: current.countJump })), {});
    const topAddicters = Object.keys(records)
        .map((userId) => {
        const playTime = records[userId].map1 ?
            records[userId].map1.playtime :
            0;
        return { userId, playTime };
    })
        .sort((a, b) => b.playTime - a.playTime)
        .slice(0, 10)
        .reduce((acc, current) => (Object.assign(Object.assign({}, acc), { [current.userId]: current.playTime })), {});
    await maxJumpRef.set(topJumpers);
    await addicterRef.set(topAddicters);
    console.log("After 6hours Updated Ranking_2sections");
    return null;
});
//# sourceMappingURL=index.js.map