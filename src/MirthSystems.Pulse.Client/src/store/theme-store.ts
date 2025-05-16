import { create } from 'zustand';

interface ThemeState {
  isDarkMode: boolean;
  toggleTheme: () => void;
  setDarkMode: (isDark: boolean) => void;
}

export const useThemeStore = create<ThemeState>()((set) => ({
  isDarkMode: localStorage.getItem('theme') === 'dark' || 
    (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches),
  
  toggleTheme: () => set((state) => {
    const newMode = !state.isDarkMode;
    localStorage.setItem('theme', newMode ? 'dark' : 'light');
    return { isDarkMode: newMode };
  }),
  
  setDarkMode: (isDark: boolean) => set(() => {
    localStorage.setItem('theme', isDark ? 'dark' : 'light');
    return { isDarkMode: isDark };
  }),
}));
