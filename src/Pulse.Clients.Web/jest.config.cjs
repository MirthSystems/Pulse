module.exports = {
    displayName: "TypeScript",
    globals: {
        __PORT__: 3000,
        __STARTCMD__: "npm run dev",
    },
    preset: "../../e2eTestUtils/jest-puppeteer-utils/jest-preset.js",
    transform: {
        "^.+\\.tsx?$": "ts-jest"
    },
    moduleNameMapper: {
        "^@/(.*)$": "<rootDir>/src/$1"
    },
    setupFilesAfterEnv: [
        "<rootDir>/test/setupTests.ts"
    ]
};
