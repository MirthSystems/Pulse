import { useState, useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router";
import { Input, Button } from "@heroui/react";
import { SearchIcon } from "@/components/icons";
import { title } from "@/components/primitives";

export default function SearchPage() {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const [searchQuery, setSearchQuery] = useState("");
  const query = searchParams.get("q") || "";

  useEffect(() => {
    setSearchQuery(query);
  }, [query]);

  const handleSearch = () => {
    if (searchQuery.trim()) {
      navigate(`/search?q=${encodeURIComponent(searchQuery)}`);
    }
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === "Enter") {
      handleSearch();
    }  };
    return (
    <section className="flex flex-col items-center justify-center gap-4 py-8 md:py-10">
      <div className="w-full max-w-2xl text-center mx-auto">
        <h1 className={title()}>Search Results</h1>
        
        <div className="flex gap-2 mt-6">
          <Input
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            onKeyPress={handleKeyPress}
            placeholder="Search..."
            startContent={<SearchIcon className="text-base text-default-400 pointer-events-none flex-shrink-0" />}
            className="flex-1"
          />
          <Button color="primary" onPress={handleSearch}>
            Search
          </Button>
        </div>

        {query && (
          <div className="mt-8">
            <p className="text-default-600">Search results for: "{query}"</p>
            <div className="mt-4 p-4 border border-default-200 rounded-lg">
              <p className="text-default-500">Search functionality will be implemented here.</p>
            </div>
          </div>
        )}
      </div>
    </section>
  );
}
