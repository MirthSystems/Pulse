# GitHub Contribution Instructions

## TypeScript Coding Rules

- **Never use unexplicit `any`**: Always use explicit types. Avoid using `any` unless absolutely necessary and always prefer more specific types.
- **strict**: Always enable strict type checking in TypeScript.
- **noUnusedLocals**: Do not leave unused local variables in the code.
- **noUnusedParameters**: Do not leave unused function parameters in the code.
- **erasableSyntaxOnly**: Only use syntax that can be safely erased by the TypeScript compiler.
- **noFallthroughCasesInSwitch**: Prevent fallthrough between switch cases unless explicitly intended.
- **noUncheckedSideEffectImports**: Avoid imports that have side effects which are not checked by TypeScript.

## Enforcement

These rules are enforced in the TypeScript configuration and must be followed for all code contributions.

---

_Keep this file updated with any new coding standards or rules for this repository._
